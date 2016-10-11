namespace PsMidiProfiler.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using PsMidiProfiler.Enums;

    /// <summary>
    /// Interaction logic for RealDrumsCC4Monitor.xaml
    /// </summary>
    public partial class RealDrumsCC4Monitor : UserControl, IControllerMonitor, INotifyPropertyChanged, ICC4HiHatPedalMonitor
    {
        private PsDevice device;

        private List<MonitorButton> monitorButtons;

        private byte hihatPedalVelocity;

        private Visibility redVisibility;

        private Visibility rimVisibility;

        private Visibility yellowClosedVisibility;

        private Visibility yellowOpenVisibility;

        private Visibility yellowPedalVisibility;

        private Visibility yellowSizzleVisibility;

        private Visibility yellowTomVisibility;

        private Visibility blueVisibility;

        private Visibility blueTomVisibility;

        private Visibility greenVisibility;

        private Visibility greenTomVisibility;

        private Visibility bassVisibility;

        public RealDrumsCC4Monitor()
        {
            this.InitializeComponent();
            var buttons = new List<ButtonName>();
            buttons.Add(ButtonName.Red);
            buttons.Add(ButtonName.Rim);
            buttons.Add(ButtonName.Yellow);
            buttons.Add(ButtonName.Yellow_Tom);
            buttons.Add(ButtonName.Blue);
            buttons.Add(ButtonName.Blue_Tom);
            buttons.Add(ButtonName.Green);
            buttons.Add(ButtonName.Green_Tom);
            buttons.Add(ButtonName.Bass);
            buttons.Add(ButtonName.Bass);

            this.device = new PsDevice("Real Drums CC#4 Midi Profile", DeviceType.DrumsReal);
            this.device.Method = (int)Method.CC4HiHat;
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
            this.RimVisibility = Visibility.Hidden;
            this.YellowClosedVisibility = Visibility.Hidden;
            this.YellowOpenVisibility = Visibility.Hidden;
            this.YellowPedalVisibility = Visibility.Hidden;
            this.YellowSizzleVisibility = Visibility.Hidden;
            this.YellowTomVisibility = Visibility.Hidden;
            this.BlueVisibility = Visibility.Hidden;
            this.BlueTomVisibility = Visibility.Hidden;
            this.GreenVisibility = Visibility.Hidden;
            this.GreenTomVisibility = Visibility.Hidden;
            this.BassVisibility = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ControllerType ControllerType
        {
            get { return ControllerType.RealDrums; }
        }

        public PsDevice Device
        {
            get { return this.device; }
        }

        public IEnumerable<MonitorButton> MonitorButtons
        {
            get { return this.monitorButtons; }
        }

        public byte HiHatPedalVelocity
        {
            get
            {
                return this.hihatPedalVelocity;
            }

            set
            {
                this.hihatPedalVelocity = value;
                this.OnPropertyChanged("HiHatPedalVelocity");

                if (value == 127 && this.yellowClosedVisibility != Visibility.Visible)
                {
                    // Hightligthing the closed hi-hat for a short time
                    var task = Task.Run(async delegate
                    {
                        this.YellowClosedVisibility = Visibility.Visible;
                        await Task.Delay(MidiViewModel.UnhighlightDelayInMilliseconds);
                        this.YellowClosedVisibility = Visibility.Hidden;
                    }); 
                }
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

        public Visibility RimVisibility
        {
            get
            {
                return this.rimVisibility;
            }

            set
            {
                this.rimVisibility = value;
                this.OnPropertyChanged("RimVisibility");
            }
        }

        public Visibility YellowClosedVisibility
        {
            get
            {
                return this.yellowClosedVisibility;
            }

            set
            {
                this.yellowClosedVisibility = value;
                this.OnPropertyChanged("YellowClosedVisibility");
            }
        }

        public Visibility YellowOpenVisibility
        {
            get
            {
                return this.yellowOpenVisibility;
            }

            set
            {
                this.yellowOpenVisibility = value;
                this.OnPropertyChanged("YellowOpenVisibility");
            }
        }

        public Visibility YellowPedalVisibility
        {
            get
            {
                return this.yellowPedalVisibility;
            }

            set
            {
                this.yellowPedalVisibility = value;
                this.OnPropertyChanged("YellowPedalVisibility");
            }
        }

        public Visibility YellowSizzleVisibility
        {
            get
            {
                return this.yellowSizzleVisibility;
            }

            set
            {
                this.yellowSizzleVisibility = value;
                this.OnPropertyChanged("YellowSizzleVisibility");
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

        public void Highlight(ButtonName button, bool value)
        {
            Visibility result = PsMidiProfiler.Convert.ToVisibility(value);

            if (button == ButtonName.Red)
            {
                this.RedVisibility = result;
            }
            else if (button == ButtonName.Rim)
            {
                this.RimVisibility = result;
            }
            else if (button == ButtonName.Yellow_Tom)
            {
                this.YellowTomVisibility = result;
            }
            else if (button == ButtonName.Blue)
            {
                this.BlueVisibility = result;
            }
            else if (button == ButtonName.Blue_Tom)
            {
                this.BlueTomVisibility = result;
            }
            else if (button == ButtonName.Green)
            {
                this.GreenVisibility = result;
            }
            else if (button == ButtonName.Green_Tom)
            {
                this.GreenTomVisibility = result;
            }
            else if (button == ButtonName.Bass)
            {
                this.BassVisibility = result;
            }
            else if (button == ButtonName.Yellow)
            {
                var hihatState = this.GetHiHatState(this.HiHatPedalVelocity);
                switch (hihatState)
                {
                    case HiHatState.Closed:
                        this.YellowClosedVisibility = result;
                        break;
                    case HiHatState.HalfClosed:
                        this.YellowSizzleVisibility = result;
                        break;
                    case HiHatState.Opened:
                        this.YellowOpenVisibility = result;
                        break;
                    default:
                        throw new NotImplementedException("Not implemented HiHatState: " + hihatState);
                }
            }
        }

        private HiHatState GetHiHatState(byte velocity)
        {
            if (velocity >= 120)
            {
                return HiHatState.Closed;
            }
            else if (velocity >= 80)
            {
                return HiHatState.HalfClosed;
            }
            else
            {
                return HiHatState.Opened;
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
