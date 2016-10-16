namespace PsMidiProfiler.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using PsMidiProfiler.Commands;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;

    /// <summary>
    /// Interaction logic for CustomControllerMonitor.xaml
    /// </summary>
    public partial class CustomControllerMonitor : UserControl, IControllerMonitor, IButtonHighlighter, INotifyPropertyChanged
    {
        private Controller controller;

        private PsDevice device;

        private List<MonitorButton> monitorButtons;

        private ICommand addButtonCommand;

        private DeviceType currentDeviceType;

        private Method currentMethod;

        public CustomControllerMonitor()
        {
            this.InitializeComponent();
            this.controller = new Controller(ControllerType.CustomController, ControllerCategory.Custom);
            this.device = new PsDevice("Custom Controller Midi Profile", DeviceType.Gamepad);
            this.device.ProfileButtons = new List<PsProfileButton>();

            this.CurrentDeviceType = (DeviceType)this.device.Type;
            this.CurrentMethod = (Method)this.device.Method;

            this.monitorButtons = new List<MonitorButton>();
            this.RemoveableButtons = new ObservableCollection<RemoveableMonitorButton>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Controller Controller
        {
            get { return this.controller; }
        }

        public PsDevice Device
        {
            get { return this.device; }
        }

        public IEnumerable<MonitorButton> MonitorButtons
        {
            get
            {
                return this.monitorButtons;
            }

            set
            {
                this.monitorButtons = new List<MonitorButton>(value);
                this.OnPropertyChanged("MonitorButtons");
            }
        }

        public ObservableCollection<RemoveableMonitorButton> RemoveableButtons { get; set; }

        public ICommand AddButtonCommand
        {
            get
            {
                if (this.addButtonCommand == null)
                {
                    this.addButtonCommand = new RelayCommand(this.AddButtonRequested);
                }

                return this.addButtonCommand;
            }
        }

        public IEnumerable<DeviceType> DeviceTypes
        {
            get
            {
                return Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>();
            }
        }

        public IEnumerable<Method> Methods
        {
            get
            {
                return Enum.GetValues(typeof(Method)).Cast<Method>();
            }
        }

        public DeviceType CurrentDeviceType
        {
            get
            {
                return this.currentDeviceType;
            }

            set
            {
                this.currentDeviceType = value;
                this.device.Type = (int)value;
                this.OnPropertyChanged("CurrentDeviceType");
            }
        }

        public Method CurrentMethod
        {
            get
            {
                return this.currentMethod;
            }

            set
            {
                this.currentMethod = value;
                this.device.Method = (int)value;
                this.OnPropertyChanged("CurrentMethod");
            }
        }

        public Color AddButtonBorderColor
        {
            get
            {
                if (this.RemoveableButtons == null || this.RemoveableButtons.Count == 0)
                {
                    return Colors.Red;
                }

                return Colors.Transparent;
            }
        }

        public void Highlight(ButtonName button, bool isNoteOn, byte velocity)
        {
            if (button == ButtonName.None)
            {
                return;
            }

            foreach (var removeableButton in this.RemoveableButtons)
            {
                if (removeableButton.MonitorButton.ProfileButton.Name == button)
                {
                    removeableButton.Highlight(isNoteOn);
                }
            }
        }

        private void AddButtonRequested(object obj)
        {
            var profileButton = new PsProfileButton(ButtonName.None, 0, 0, 0);
            var monitorButton = new MonitorButton(profileButton);
            var removeableButton = new RemoveableMonitorButton(monitorButton);
            removeableButton.Removed += this.OnRemoveableMonitorButtonRemoved;

            this.device.ProfileButtons.Add(profileButton);
            this.monitorButtons.Add(monitorButton);
            this.RemoveableButtons.Add(removeableButton);

            this.OnPropertyChanged("RemoveableButtons");
            this.OnPropertyChanged("AddButtonBorderColor");
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnRemoveableMonitorButtonRemoved(object sender, EventArgs e)
        {
            var removeableButton = sender as RemoveableMonitorButton;
            if (removeableButton == null)
            {
                return;
            }

            var profileButton = removeableButton.MonitorButton.ProfileButton;
            var monitorButton = this.monitorButtons.FirstOrDefault(x => x.ProfileButton == profileButton);
            if (monitorButton == null)
            {
                return;
            }

            this.device.ProfileButtons.Remove(profileButton);
            this.monitorButtons.Remove(monitorButton);
            this.RemoveableButtons.Remove(removeableButton);

            this.OnPropertyChanged("RemoveableButtons");
            this.OnPropertyChanged("AddButtonBorderColor");
        }
    }
}