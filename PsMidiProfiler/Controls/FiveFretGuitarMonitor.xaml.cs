namespace PsMidiProfiler.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;

    /// <summary>
    /// Interaction logic for FiveFretGuitarMonitor.xaml
    /// </summary>
    public partial class FiveFretGuitarMonitor : UserControl, IControllerMonitor, IButtonHighlighter, IPitchBendMonitor, INotifyPropertyChanged
    {
        private PsDevice device;

        private List<MonitorButton> monitorButtons;

        private float pitchWheelValue;

        private Visibility greenVisibility;
        
        private Visibility redVisibility;
        
        private Visibility yellowVisibility;
        
        private Visibility blueVisibility;
        
        private Visibility orangeVisibility;
        
        private Visibility upVisibility;
        
        private Visibility downVisibility;
        
        private Visibility startVisibility;
        
        private Visibility backVisibility;

        public FiveFretGuitarMonitor()
        {
            this.InitializeComponent();

            var buttons = new List<ButtonName>();
            buttons.Add(ButtonName.Green);
            buttons.Add(ButtonName.Red);
            buttons.Add(ButtonName.Yellow);
            buttons.Add(ButtonName.Blue);
            buttons.Add(ButtonName.Orange);
            buttons.Add(ButtonName.Up);
            buttons.Add(ButtonName.Down);
            buttons.Add(ButtonName.Start);
            buttons.Add(ButtonName.Back);

            this.device = new PsDevice("Five Fret Whammy Guitar Midi Profile", Enums.DeviceType.Guitar);
            this.device.Method = (int)Method.GuitarLegacy;
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
            this.UpVisibility = Visibility.Hidden;
            this.DownVisibility = Visibility.Hidden;
            this.StartVisibility = Visibility.Hidden;
            this.BackVisibility = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PsDevice Device
        {
            get { return this.device; }
        }

        public IEnumerable<MonitorButton> MonitorButtons
        {
            get { return this.monitorButtons; }
        }

        public float PitchWheelValue
        {
            get
            {
                return this.pitchWheelValue;
            }

            set
            {
                this.pitchWheelValue = (float)Math.Round((value + 1) / 2.0f, 2) * 100.0f;
                this.OnPropertyChanged("PitchWheelValue");
            }
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

        public Visibility UpVisibility
        {
            get
            {
                return this.upVisibility;
            }

            set
            {
                this.upVisibility = value;
                this.OnPropertyChanged("UpVisibility");
            }
        }

        public Visibility DownVisibility
        {
            get
            {
                return this.downVisibility;
            }

            set
            {
                this.downVisibility = value;
                this.OnPropertyChanged("DownVisibility");
            }
        }

        public Visibility StartVisibility
        {
            get
            {
                return this.startVisibility;
            }

            set
            {
                this.startVisibility = value;
                this.OnPropertyChanged("StartVisibility");
            }
        }

        public Visibility BackVisibility
        {
            get
            {
                return this.backVisibility;
            }

            set
            {
                this.backVisibility = value;
                this.OnPropertyChanged("BackVisibility");
            }
        }

        public void Highlight(ButtonName button, bool isNoteOn, byte velocity)
        {
            var result = Helpers.Convert.ToVisibility(isNoteOn);

            if (button == ButtonName.Green)
            {
                this.GreenVisibility = result;
            }
            else if (button == ButtonName.Red)
            {
                this.RedVisibility = result;
            }
            else if (button == ButtonName.Yellow)
            {
                this.YellowVisibility = result;
            }
            else if (button == ButtonName.Blue)
            {
                this.BlueVisibility = result;
            }
            else if (button == ButtonName.Orange)
            {
                this.OrangeVisibility = result;
            }
            else if (button == ButtonName.Up)
            {
                this.UpVisibility = result;
            }
            else if (button == ButtonName.Down)
            {
                this.DownVisibility = result;
            }
            else if (button == ButtonName.Start)
            {
                this.StartVisibility = result;
            }
            else if (button == ButtonName.Back)
            {
                this.BackVisibility = result;
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