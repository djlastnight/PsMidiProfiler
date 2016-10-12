namespace PsMidiProfiler.Controls
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Helpers;
    using PsMidiProfiler.Models;

    /// <summary>
    /// Interaction logic for AdvancedDrumsMonitor.xaml
    /// </summary>
    public partial class RealDrumsMonitor : UserControl, IControllerMonitor, INotifyPropertyChanged
    {
        private PsDevice device;

        private List<MonitorButton> monitorButtons;

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

        public RealDrumsMonitor()
        {
            this.InitializeComponent();
            var buttons = new List<ButtonName>();
            buttons.Add(ButtonName.Red);

            // buttons.Add(ButtonName.Rim);
            buttons.Add(ButtonName.Yellow_C);
            buttons.Add(ButtonName.Yellow_O);
            buttons.Add(ButtonName.Yellow_P);
            buttons.Add(ButtonName.Yellow_S);
            buttons.Add(ButtonName.Yellow_Tom);
            buttons.Add(ButtonName.Blue);
            buttons.Add(ButtonName.Blue_Tom);
            buttons.Add(ButtonName.Green);
            buttons.Add(ButtonName.Green_Tom);
            buttons.Add(ButtonName.Bass);
            buttons.Add(ButtonName.Bass);

            this.device = new PsDevice("Real Drums Midi Profile", DeviceType.DrumsReal);
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

        public Controller Controller
        {
            get
            {
                return new Controller(ControllerType.RealDrums, ControllerCategory.Drums);
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
            Visibility result = Convert.ToVisibility(value);

            if (button == ButtonName.Red)
            {
                this.RedVisibility = result;
            }
            else if (button == ButtonName.Yellow_C)
            {
                this.YellowClosedVisibility = result;
            }
            else if (button == ButtonName.Yellow_O)
            {
                this.YellowOpenVisibility = result;
            }
            else if (button == ButtonName.Yellow_P)
            {
                this.YellowPedalVisibility = result;
            }
            else if (button == ButtonName.Yellow_S)
            {
                this.YellowSizzleVisibility = result;
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
