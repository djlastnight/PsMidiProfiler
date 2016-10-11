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
    /// Interaction logic for FiveFretKeys.xaml
    /// </summary>
    public partial class FiveFretKeysMonitor : UserControl, IControllerMonitor, INotifyPropertyChanged
    {
        private PsDevice device;

        private List<MonitorButton> monitorButtons;

        private Visibility greenVisibility;

        private Visibility redVisibility;

        private Visibility yellowVisibility;

        private Visibility blueVisibility;

        private Visibility orangeVisibility;

        public FiveFretKeysMonitor()
        {
            this.InitializeComponent();
            var buttons = new List<ButtonName>();
            buttons.Add(ButtonName.Green);
            buttons.Add(ButtonName.Red);
            buttons.Add(ButtonName.Yellow);
            buttons.Add(ButtonName.Blue);
            buttons.Add(ButtonName.Orange);

            this.device = new PsDevice("Five Fret Keys Midi Profile", DeviceType.Keys);
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

        public ControllerType ControllerType
        {
            get { throw new NotImplementedException(); }
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

        public void Highlight(ButtonName button, bool value)
        {
            var result = Helpers.Convert.ToVisibility(value);
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