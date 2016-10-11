namespace PsMidiProfiler.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using PsMidiProfiler.Enums;
    using radio42.Multimedia.Midi;

    /// <summary>
    /// Interaction logic for NoteWaiter.xaml
    /// </summary>
    public partial class NoteDetector : Window, INotifyPropertyChanged
    {
        private MidiViewModel midiViewModel;

        private ButtonName buttonName;

        private int detectedNote;

        private int detectedChannel;

        private int detectedNoteOffValue;

        private string status;

        public NoteDetector(MidiViewModel midiViewModel, ButtonName buttonName)
            : this()
        {
            this.MidiViewModel = midiViewModel;
            this.midiViewModel.MidiModel.MessageReceived += this.MidiMessageReceived;
            this.ButtonName = buttonName;
            this.Status = "Waiting for midi note on event";
        }

        private NoteDetector()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event NoteDetectedEventHandler NoteDetected;

        public MidiViewModel MidiViewModel
        {
            get
            {
                return this.midiViewModel;
            }

            set
            {
                this.midiViewModel = value;
                this.OnPropertyChanged("MidiViewModel");
            }
        }

        public ButtonName ButtonName
        {
            get
            {
                return this.buttonName;
            }

            set
            {
                this.buttonName = value;
                this.OnPropertyChanged("ButtonName");
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.OnPropertyChanged("Status");
            }
        }

        private void MidiMessageReceived(object sender, MidiMessageEventArgs e)
        {
            if (!e.IsShortMessage)
            {
                return;
            }

            if (this.detectedNote == 0)
            {
                if (e.ShortMessage.StatusType == MIDIStatus.NoteOn)
                {
                    this.detectedNote = e.ShortMessage.Note;
                    this.detectedChannel = e.ShortMessage.Channel;

                    if (!this.midiViewModel.WaitForNoteOff)
                    {
                        this.OnNoteDetected();
                        return;
                    }

                    this.Status = string.Format(
                        "Detected note {0} on channel {1}.{2}Waiting for note off event..",
                        this.detectedNote,
                        this.detectedChannel,
                        Environment.NewLine);
                }

                return;
            }

            if (e.ShortMessage.StatusType == MIDIStatus.NoteOff &&
                e.ShortMessage.Note == this.detectedNote &&
                e.ShortMessage.Channel == this.detectedChannel)
            {
                this.detectedNoteOffValue = e.ShortMessage.Velocity;
                this.OnNoteDetected();
            }
        }

        private void OnNoteDetected()
        {
            if (this.NoteDetected != null)
            {
                var args = new NoteDetectedEventArgs(
                    this.detectedNote,
                    this.detectedChannel,
                    this.detectedNoteOffValue);

                this.NoteDetected(this, args);
                this.Dispatcher.Invoke((Action)delegate
                {
                    this.Close();
                });
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(
                    this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.NoteDetected == null)
            {
                throw new InvalidOperationException(
                    "You must subscribe for NoteDetected event first!");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.NoteDetected = null;
            this.midiViewModel.MidiModel.MessageReceived -= this.MidiMessageReceived;
        }
    }
}