namespace PsMidiProfiler.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using System.Windows.Input;
    using PsMidiProfiler.Commands;
    using PsMidiProfiler.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Helpers;
    using PsMidiProfiler.Models;
    using radio42.Multimedia.Midi;

    public class MidiViewModel : INotifyPropertyChanged
    {
        public const int UnhighlightDelayInMilliseconds = 150;

        private MidiModel midi;

        private Controller controller;

        private IControllerMonitor controllerMonitor;

        private ICommand clearMidiMonitorHistoryCommand;

        private ICommand generateProfileCommand;

        private bool waitForNoteOff;

        public MidiViewModel()
        {
            this.midi = new MidiModel();
            this.midi.MessageReceived += this.OnMidiMessageReceived;
            this.CurrentController = new Controller(ControllerType.FourLaneDrums, ControllerCategory.Drums);
            this.IsAudioPreviewEnabled = true;
            this.WaitForNoteOff = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ProfileGeneratedEventArgs> ProfileGenerated;

        public MidiModel MidiModel
        {
            get
            {
                return this.midi;
            }
        }

        public bool IsAudioPreviewEnabled
        {
            get
            {
                return MidiModel.IsOutputEnabled;
            }

            set
            {
                MidiModel.IsOutputEnabled = value;
                this.OnPropertyChanged("IsAudioPreviewEnabled");
            }
        }

        public IEnumerable<string> MidiInDevices
        {
            get
            {
                return this.midi.MidiInDevices;
            }
        }

        public string CurrentMidiInDevice
        {
            get
            {
                return this.midi.CurrentDevice;
            }

            set
            {
                this.midi.CurrentDevice = value;
            }
        }

        public string NoteHistory
        {
            get
            {
                return this.midi.NoteHistory;
            }
        }

        public IControllerMonitor ControllerMonitor
        {
            get
            {
                return this.controllerMonitor;
            }

            private set
            {
                this.controllerMonitor = value;
                this.OnPropertyChanged("ControllerMonitor");
            }
        }

        public ListCollectionView Controllers
        {
            get
            {
                var controllers = Helpers.ControllerHelper.GetControllers();
                var list = new ListCollectionView(controllers);
                list.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
                return list;
            }
        }

        public Controller CurrentController
        {
            get
            {
                return this.controller;
            }

            set
            {
                this.controller = value;
                this.controllerMonitor = Helpers.ControllerHelper.CreateMonitor(this.controller);

                if (this.controllerMonitor is IButtonHighlighter)
                {
                    var buttons = (this.controllerMonitor as IButtonHighlighter).MonitorButtons;
                    foreach (var button in buttons)
                    {
                        button.Cleared += this.OnMonitorButtonCleared;
                    }
                }

                this.ControllerMonitor = this.controllerMonitor;
                this.OnPropertyChanged("CurrentController");
            }
        }

        public bool WaitForNoteOff
        {
            get
            {
                return this.waitForNoteOff;
            }

            set
            {
                this.waitForNoteOff = value;
                this.OnPropertyChanged("WaitForNoteOff");
            }
        }

        public IEnumerable<bool> Booleans
        {
            get
            {
                return new List<bool>()
                {
                    true,
                    false
                };
            }
        }

        public ICommand ClearMidiMonitorHistoryCommand
        {
            get
            {
                if (this.clearMidiMonitorHistoryCommand == null)
                {
                    this.clearMidiMonitorHistoryCommand = new RelayCommand(
                        this.OnClearMonitorHistoryRequested);
                }

                return this.clearMidiMonitorHistoryCommand;
            }
        }

        public ICommand GenerateProfileCommand
        {
            get
            {
                if (this.generateProfileCommand == null)
                {
                    this.generateProfileCommand = new RelayCommand(this.OnGenerateProfileRequested);
                }

                return this.generateProfileCommand;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnClearMonitorHistoryRequested(object obj)
        {
            this.midi.ClearNoteHistory();
            this.OnPropertyChanged("NoteHistory");
        }

        private void OnGenerateProfileRequested(object obj)
        {
            bool checkButtons = true;

            if (obj is bool)
            {
                checkButtons = (bool)obj;
            }

            var midiProfile = ProfileCreator.Create(this.controllerMonitor, checkButtons);
            if (this.ProfileGenerated != null)
            {
                this.ProfileGenerated(this, new ProfileGeneratedEventArgs(midiProfile));
            }
        }

        private void OnMidiMessageReceived(object sender, MidiMessageEventArgs e)
        {
            this.OnPropertyChanged("NoteHistory");
            if (!e.IsShortMessage)
            {
                return;
            }

            byte velocity = e.ShortMessage.Velocity;
            bool isControlChange = e.ShortMessage.StatusType == MIDIStatus.ControlChange;
            bool isPitchBend = e.ShortMessage.StatusType == MIDIStatus.PitchBend;
            bool isNoteOn = e.ShortMessage.StatusType == MIDIStatus.NoteOn;
            bool isNoteOff = e.ShortMessage.StatusType == MIDIStatus.NoteOff;

            if (isControlChange && this.controllerMonitor is IHiHatPedalMonitor)
            {
                byte controller = e.ShortMessage.Controller;
                bool isMSB = controller == 4;
                bool isLSB = controller == 36;

                if (isMSB || (isLSB && velocity == 0))
                {
                    var pedalMonitor = this.controllerMonitor as IHiHatPedalMonitor;
                    pedalMonitor.HiHatPedalVelocity = velocity;
                }
            }

            if (isPitchBend && this.controllerMonitor is IPitchBendMonitor)
            {
                var pitchMonitor = this.controllerMonitor as IPitchBendMonitor;
                pitchMonitor.PitchWheelValue = (float)Math.Round((e.ShortMessage.PitchBend - 8192) / 8192.0f, 2);
            }

            if (!isNoteOn && !isNoteOff)
            {
                return;
            }

            if (this.controllerMonitor is INoteHighlighter)
            {
                var noteHighlighter = this.controllerMonitor as INoteHighlighter;
                noteHighlighter.HighlightNote(
                    e.ShortMessage.Note,
                    isNoteOn,
                    e.ShortMessage.Velocity);
                return;
            }

            if (this.controllerMonitor is IButtonHighlighter)
            {
                var buttonHighlighter = this.controllerMonitor as IButtonHighlighter;
                var buttons = buttonHighlighter.MonitorButtons.Where(
                    button => button.ProfileButton.Note == e.ShortMessage.Note &&
                        button.ProfileButton.Channel == e.ShortMessage.Channel);

                foreach (var button in buttons)
                {
                    buttonHighlighter.Highlight(
                        button.ProfileButton.Name,
                        isNoteOn,
                        e.ShortMessage.Velocity);
                }

                if (isNoteOn && !this.WaitForNoteOff)
                {
                    var task = Task.Run(async delegate
                    {
                        await Task.Delay(MidiViewModel.UnhighlightDelayInMilliseconds);
                        foreach (var button in buttons)
                        {
                            buttonHighlighter.Highlight(button.ProfileButton.Name, false, 0);
                        }
                    });
                }
            }
        }

        private void OnMonitorButtonCleared(object sender, EventArgs e)
        {
            var button = sender as MonitorButton;
            var highlighter = this.controllerMonitor as IButtonHighlighter;
            if (button != null && highlighter != null)
            {
                highlighter.Highlight(button.ProfileButton.Name, false, 0);
            }
        }
    }
}