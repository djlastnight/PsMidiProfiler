namespace PsMidiProfiler.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using PsMidiProfiler.Commands;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;

    /// <summary>
    /// Interaction logic for RemoveableButton.xaml
    /// </summary>
    public partial class RemoveableMonitorButton : UserControl, INotifyPropertyChanged
    {
        private MonitorButton monitorButton;

        private ICommand removeCommand;

        private Color edgeColor;

        private Color defaultColor = Colors.White;

        private Color hightlightColor = Colors.LightYellow;

        public RemoveableMonitorButton()
        {
            this.InitializeComponent();

            this.EdgeColor = this.defaultColor;
            this.MonitorButton = new MonitorButton(new PsProfileButton(ButtonName.None, 0, 0, 0));
        }

        public RemoveableMonitorButton(MonitorButton monitorButton)
            : this()
        {
            this.MonitorButton = monitorButton;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler Removed;

        public MonitorButton MonitorButton
        {
            get
            {
                return this.monitorButton;
            }

            set
            {
                this.monitorButton = value;
                this.OnPropertyChanged("MonitorButton");
            }
        }

        public IEnumerable<ButtonName> ButtonNames
        {
            get
            {
                return Enum.GetValues(typeof(ButtonName)).Cast<ButtonName>();
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                if (this.removeCommand == null)
                {
                    this.removeCommand = new RelayCommand(this.OnRemoveRequested);
                }

                return this.removeCommand;
            }
        }

        public Color EdgeColor
        {
            get
            {
                return this.edgeColor;
            }

            set
            {
                this.edgeColor = value;
                this.OnPropertyChanged("EdgeColor");
            }
        }

        public void Highlight(bool isNoteOn)
        {
            if (isNoteOn)
            {
                this.EdgeColor = this.hightlightColor;
            }
            else
            {
                this.EdgeColor = this.defaultColor;
            }
        }

        private void OnRemoveRequested(object obj)
        {
            if (this.Removed != null)
            {
                this.Removed(this, EventArgs.Empty);
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
