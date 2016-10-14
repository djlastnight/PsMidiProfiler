namespace PsMidiProfiler.Controls
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Controls;
    using System.Windows.Input;
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

        private ObservableCollection<MonitorButton> monitorButtons;

        private ICommand addButtonCommand;

        private ICommand removeButtonCommand;

        private DeviceType currentDeviceType;

        public CustomControllerMonitor()
        {
            this.InitializeComponent();
            this.controller = new Controller(ControllerType.CustomController, ControllerCategory.Custom);
            this.device = new PsDevice("Custom Controller Midi Profile", DeviceType.Gamepad);
            this.monitorButtons = new ObservableCollection<MonitorButton>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ButtonName selectedButtonName;

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
                this.monitorButtons = new ObservableCollection<MonitorButton>(value);
                this.OnPropertyChanged("MonitorButtons");
            }
        }

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

        public ICommand RemoveButtonCommand
        {
            get
            {
                if (this.removeButtonCommand == null)
                {
                    this.removeButtonCommand = new RelayCommand(this.RemoveButtonRequested);
                }

                return this.removeButtonCommand;
            }
        }

        public ButtonName SelectedButtonName
        {
            get
            {
                return this.selectedButtonName;
            }

            set
            {
                this.selectedButtonName = value;
                this.OnPropertyChanged("SelectedButtonName");
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
                this.OnPropertyChanged("CurrentDeviceType");
            }
        }

        public IEnumerable<ButtonName> ButtonNames
        {
            get
            {
                return Enum.GetValues(typeof(ButtonName)).Cast<ButtonName>();
            }
        }

        public void Highlight(ButtonName button, bool isNoteOn, byte velocity)
        {
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void AddButtonRequested(object obj)
        {
            throw new NotImplementedException();
        }

        private void RemoveButtonRequested(object obj)
        {
            throw new NotImplementedException();
        }
    }
}