﻿using PsMidiProfiler.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PsMidiProfiler.Controls
{
    /// <summary>
    /// Interaction logic for FiveLaneDrumsMonitor.xaml
    /// </summary>
    public partial class FiveLaneDrumsMonitor : UserControl, IControllerMonitor, INotifyPropertyChanged
    {
        private PsDevice device;

        private List<MonitorButton> monitorButtons;

        private Visibility redVisibility;

        private Visibility yellowVisibility;

        private Visibility blueVisibility;

        private Visibility orangeVisibility;

        private Visibility greenVisibility;

        private Visibility bassVisibility;

        public FiveLaneDrumsMonitor()
        {
            InitializeComponent();
            var buttons = new List<ButtonName>();
            buttons.Add(ButtonName.Red);
            buttons.Add(ButtonName.Yellow);
            buttons.Add(ButtonName.Blue_Tom);
            buttons.Add(ButtonName.Orange);
            buttons.Add(ButtonName.Green_Tom);
            buttons.Add(ButtonName.Bass);
            buttons.Add(ButtonName.Bass);

            this.device = new PsDevice("Five Lane Drums Midi Profile", DeviceType.Drums);
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
            this.YellowVisibility = Visibility.Hidden;
            this.BlueVisibility = Visibility.Hidden;
            this.OrangeVisibility = Visibility.Hidden;
            this.GreenVisibility = Visibility.Hidden;
            this.BassVisibility = Visibility.Hidden;
        }

        public ControllerType ControllerType
        {
            get { return ControllerType.FiveLaneDrums; }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void Highlight(ButtonName button, bool value)
        {
            Visibility result = Convert.ToVisibility(value);

            if (button == ButtonName.Red)
            {
                this.RedVisibility = result;
            }
            else if (button == ButtonName.Yellow)
            {
                this.YellowVisibility = result;
            }
            else if (button == ButtonName.Blue_Tom)
            {
                this.BlueVisibility = result;
            }
            else if (button == ButtonName.Orange)
            {
                this.OrangeVisibility = result;
            }
            else if (button == ButtonName.Green_Tom)
            {
                this.GreenVisibility = result;
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
