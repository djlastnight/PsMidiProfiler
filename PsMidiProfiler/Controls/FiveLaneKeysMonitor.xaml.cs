namespace PsMidiProfiler.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;
    using radio42.Multimedia.Midi;

    /// <summary>
    /// Interaction logic for FiveFretKeys.xaml
    /// </summary>
    public partial class FiveLaneKeysMonitor : UserControl, IControllerMonitor, IButtonHighlighter, INotifyPropertyChanged
    {
        private PsDevice device;

        private List<MonitorButton> monitorButtons;

        private Visibility greenVisibility;

        private Visibility redVisibility;

        private Visibility yellowVisibility;

        private Visibility blueVisibility;

        private Visibility orangeVisibility;

        public FiveLaneKeysMonitor()
        {
            this.InitializeComponent();
            var buttons = new List<ButtonName>();
            buttons.Add(ButtonName.Green);
            buttons.Add(ButtonName.Red);
            buttons.Add(ButtonName.Yellow);
            buttons.Add(ButtonName.Blue);
            buttons.Add(ButtonName.Orange);

            this.device = new PsDevice("Five Lane Keys Midi Profile", DeviceType.Piano);
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

            this.GreenVisibility = Visibility.Hidden;
            this.RedVisibility = Visibility.Hidden;
            this.YellowVisibility = Visibility.Hidden;
            this.BlueVisibility = Visibility.Hidden;
            this.OrangeVisibility = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Controller Controller
        {
            get
            {
                return new Controller(ControllerType.FiveLaneKeys, ControllerCategory.Keys);
            }
        }

        public PsDevice Device
        {
            get { return this.device; }
        }

        public IEnumerable<MonitorButton> MonitorButtons
        {
            get { return this.monitorButtons; }
        }

        public Visibility GreenVisibility
        {
            get
            {
                return this.greenVisibility;
            }

            set
            {
                this.greenVisibility = value;
                this.OnPropertyChanged("GreenVisibility");
            }
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

        public Visibility YellowVisibility
        {
            get
            {
                return this.yellowVisibility;
            }

            set
            {
                this.yellowVisibility = value;
                this.OnPropertyChanged("YellowVisibility");
            }
        }

        public Visibility BlueVisibility
        {
            get
            {
                return this.blueVisibility;
            }

            set
            {
                this.blueVisibility = value;
                this.OnPropertyChanged("BlueVisibility");
            }
        }

        public Visibility OrangeVisibility
        {
            get
            {
                return this.orangeVisibility;
            }

            set
            {
                this.orangeVisibility = value;
                this.OnPropertyChanged("OrangeVisibility");
            }
        }

        public void Highlight(ButtonName button, bool isNoteOn, byte velocity)
        {
            var result = Helpers.Convert.ToVisibility(isNoteOn);
            var status = isNoteOn ? MIDIStatus.NoteOn : MIDIStatus.NoteOff;
            byte note = 0;

            if (button == ButtonName.Green)
            {
                this.GreenVisibility = result;
                note = 50;
            }
            else if (button == ButtonName.Red)
            {
                this.RedVisibility = result;
                note = 52;
            }
            else if (button == ButtonName.Yellow)
            {
                this.YellowVisibility = result;
                note = 53;
            }
            else if (button == ButtonName.Blue)
            {
                this.BlueVisibility = result;
                note = 55;
            }
            else if (button == ButtonName.Orange)
            {
                this.OrangeVisibility = result;
                note = 57;
            }

            if (note != 0)
            {
                MidiModel.Send(new MidiShortMessage(status, 1, note, velocity, 0));
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