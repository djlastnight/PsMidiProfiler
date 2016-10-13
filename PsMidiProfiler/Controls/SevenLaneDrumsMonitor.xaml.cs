namespace PsMidiProfiler.Controls
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Helpers;
    using PsMidiProfiler.Models;
    using radio42.Multimedia.Midi;

    /// <summary>
    /// Interaction logic for RealDrumsMonitor.xaml
    /// </summary>
    public partial class SevenLaneDrumsMonitor : UserControl, IControllerMonitor, IButtonHighlighter, INotifyPropertyChanged
    {
        private PsDevice device;

        private List<MonitorButton> monitorButtons;

        private Visibility redVisibility;

        private Visibility yellowTomVisibility;

        private Visibility blueTomVisibility;

        private Visibility greenTomVisibility;

        private Visibility yellowCymbalVisibility;

        private Visibility greenCymbalVisibility;

        private Visibility blueCymbalVisibility;

        private Visibility bassVisibility;

        public SevenLaneDrumsMonitor()
        {
            this.InitializeComponent();
            var buttons = new List<ButtonName>();
            buttons.Add(ButtonName.Red);
            buttons.Add(ButtonName.Yellow_Tom);
            buttons.Add(ButtonName.Blue_Tom);
            buttons.Add(ButtonName.Green_Tom);
            buttons.Add(ButtonName.Yellow);
            buttons.Add(ButtonName.Green);
            buttons.Add(ButtonName.Blue);
            buttons.Add(ButtonName.Bass);
            buttons.Add(ButtonName.Bass);

            this.device = new PsDevice("Seven Lane Drums Midi Profile", DeviceType.Drums);
            this.device.ProfileButtons = new List<PsProfileButton>();
            this.monitorButtons = new List<MonitorButton>();

            foreach (var button in buttons)
            {
                var profileButton = new PsProfileButton(button, 0, 0, 0);
                var monitorButton = new MonitorButton(profileButton);

                this.device.ProfileButtons.Add(profileButton);
                this.monitorButtons.Add(monitorButton);
            }

            this.OnPropertyChanged("MonitorButtons");

            this.RedVisibility = Visibility.Hidden;
            this.YellowTomVisibility = Visibility.Hidden;
            this.BlueTomVisibility = Visibility.Hidden;
            this.GreenTomVisibility = Visibility.Hidden;
            this.YellowCymbalVisibility = Visibility.Hidden;
            this.GreenCymbalVisibility = Visibility.Hidden;
            this.BlueCymbalVisibility = Visibility.Hidden;
            this.BassVisibility = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Controller Controller
        {
            get { return new Controller(ControllerType.SevenLaneDrums, ControllerCategory.Drums); }
        }

        public PsDevice Device
        {
            get { return this.device; }
        }

        public IEnumerable<MonitorButton> MonitorButtons
        {
            get { return this.monitorButtons; }
        }

        public Visibility RedVisibility
        {
            get
            {
                return this.redVisibility;
            }

            set
            {
                this.redVisibility = value;
                this.OnPropertyChanged("RedVisibility");
            }
        }

        public Visibility YellowTomVisibility
        {
            get
            {
                return this.yellowTomVisibility;
            }

            set
            {
                this.yellowTomVisibility = value;
                this.OnPropertyChanged("YellowTomVisibility");
            }
        }

        public Visibility BlueTomVisibility
        {
            get
            {
                return this.blueTomVisibility;
            }

            set
            {
                this.blueTomVisibility = value;
                this.OnPropertyChanged("BlueTomVisibility");
            }
        }

        public Visibility GreenTomVisibility
        {
            get
            {
                return this.greenTomVisibility;
            }

            set
            {
                this.greenTomVisibility = value;
                this.OnPropertyChanged("GreenTomVisibility");
            }
        }

        public Visibility YellowCymbalVisibility
        {
            get
            {
                return this.yellowCymbalVisibility;
            }

            set
            {
                this.yellowCymbalVisibility = value;
                this.OnPropertyChanged("YellowCymbalVisibility");
            }
        }

        public Visibility GreenCymbalVisibility
        {
            get
            {
                return this.greenCymbalVisibility;
            }

            set
            {
                this.greenCymbalVisibility = value;
                this.OnPropertyChanged("GreenCymbalVisibility");
            }
        }

        public Visibility BlueCymbalVisibility
        {
            get
            {
                return this.blueCymbalVisibility;
            }

            set
            {
                this.blueCymbalVisibility = value;
                this.OnPropertyChanged("BlueCymbalVisibility");
            }
        }

        public Visibility BassVisibility
        {
            get
            {
                return this.bassVisibility;
            }

            set
            {
                this.bassVisibility = value;
                this.OnPropertyChanged("BassVisibility");
            }
        }

        public void Highlight(ButtonName button, bool isNoteOn, byte velocity)
        {
            Visibility result = Convert.ToVisibility(isNoteOn);
            var status = isNoteOn ? MIDIStatus.NoteOn : MIDIStatus.NoteOff;
            MidiNote note = MidiNote.None;

            if (button == ButtonName.Red)
            {
                this.RedVisibility = result;
                note = MidiNote.AcousticSnare;
            }
            else if (button == ButtonName.Yellow_Tom)
            {
                this.YellowTomVisibility = result;
                note = MidiNote.LowTom;
            }
            else if (button == ButtonName.Blue_Tom)
            {
                this.BlueTomVisibility = result;
                note = MidiNote.HighFloorTom;
            }
            else if (button == ButtonName.Green_Tom)
            {
                this.GreenTomVisibility = result;
                note = MidiNote.LowFloorTom;
            }
            else if (button == ButtonName.Yellow)
            {
                this.YellowCymbalVisibility = result;
                note = MidiNote.ClosedHiHat;
            }
            else if (button == ButtonName.Green)
            {
                this.GreenCymbalVisibility = result;
                note = MidiNote.CrashCymbal1;
            }
            else if (button == ButtonName.Blue)
            {
                this.BlueCymbalVisibility = result;
                note = MidiNote.RideCymbal2;
            }
            else if (button == ButtonName.Bass)
            {
                this.BassVisibility = result;
                note = MidiNote.BassDrum1;
            }

            if (note != MidiNote.None)
            {
                MidiModel.Send(new MidiShortMessage(status, 9, (byte)note, velocity, 0));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
