namespace PsMidiProfiler.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using PsMidiProfiler.Commands;
    using PsMidiProfiler.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Helpers;
    using PsMidiProfiler.Models;
    using radio42.Multimedia.Midi;

    public class MidiViewModel : INotifyPropertyChanged
    {
        public const int UnhighlightDelayInMilliseconds = 150;

        private MidiModel midi;

        private ControllerType currentControllerType;

        private IControllerMonitor controllerMonitor;

        private ICommand clearMidiMonitorHistoryCommand;

        private ICommand generateProfileCommand;

        private bool waitForNoteOff;

        public MidiViewModel()
        {
            this.midi = new MidiModel();
            this.midi.MessageReceived += this.OnMidiMessageReceived;
            this.CurrentControllerType = ControllerType.FourLaneDrums;
            this.WaitForNoteOff = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ProfileGeneratedEventArgs> ProfileGenerated;

        public MidiModel MidiModel
        {
            get
            {
                return this.midi;
            }
        }

        public IEnumerable<string> MidiInDevices
        {
            get
            {
                return this.midi.MidiInDevices;
            }
        }

        public string CurrentMidiInDevice
        {
            get
            {
                return this.midi.CurrentDevice;
            }

            set
            {
                this.midi.CurrentDevice = value;
            }
        }

        public string NoteHistory
        {
            get
            {
                return this.midi.NoteHistory;
            }
        }

        public IControllerMonitor ControllerMonitor
        {
            get
            {
                return this.controllerMonitor;
            }

            private set
            {
                this.controllerMonitor = value;
                this.OnPropertyChanged("ControllerMonitor");
            }
        }

        public IEnumerable<ControllerType> ControllerTypes
        {
            get
            {
                return Enum.GetValues(typeof(ControllerType)).Cast<ControllerType>();
            }
        }

        public ControllerType CurrentControllerType
        {
            get
            {
                return this.currentControllerType;
            }

            set
            {
                this.currentControllerType = value;
                switch (value)
                {
                    case ControllerType.FourLaneDrums:
                        this.controllerMonitor = new FourLaneDrumsMonitor();
                        break;
                    case ControllerType.FiveLaneDrums:
                        this.controllerMonitor = new FiveLaneDrumsMonitor();
                        break;
                    case ControllerType.RealDrums:
                        this.controllerMonitor = new RealDrumsMonitor();
                        break;
                    case ControllerType.RealDrumsCC4:
                        this.controllerMonitor = new RealDrumsCC4Monitor();
                        break;
                    case ControllerType.SevenLaneDrums:
                        this.controllerMonitor = new SevenLaneDrumsMonitor();
                        break;
                    case ControllerType.FiveFretKeys:
                        this.controllerMonitor = new FiveFretKeysMonitor();
                        break;
                    default:
                        throw new NotImplementedException("Not implemented controller type: " + value);
                }

                foreach (var button in this.controllerMonitor.MonitorButtons)
                {
                    button.Cleared += this.OnMonitorButtonCleared;
                }

                this.ControllerMonitor = this.controllerMonitor;
            }
        }

        public bool WaitForNoteOff
        {
            get
            {
                return this.waitForNoteOff;
            }

            set
            {
                this.waitForNoteOff = value;
                this.OnPropertyChanged("WaitForNoteOff");
            }
        }

        public IEnumerable<bool> Booleans
        {
            get
            {
                return new List<bool>()
                {
                    true,
                    false
                };
            }
        }

        public ICommand ClearMidiMonitorHistoryCommand
        {
            get
            {
                if (this.clearMidiMonitorHistoryCommand == null)
                {
                    this.clearMidiMonitorHistoryCommand = new RelayCommand(
                        this.OnClearMonitorHistoryRequested);
                }

                return this.clearMidiMonitorHistoryCommand;
            }
        }

        public ICommand GenerateProfileCommand
        {
            get
            {
                if (this.generateProfileCommand == null)
                {
                    this.generateProfileCommand = new RelayCommand(this.OnGenerateProfileRequested);
                }

                return this.generateProfileCommand;
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnClearMonitorHistoryRequested(object obj)
        {
            this.midi.ClearNoteHistory();
            this.OnPropertyChanged("NoteHistory");
        }

        private void OnGenerateProfileRequested(object obj)
        {
            string error;
            string midiProfile = PsDeviceSerializer.Serialize(this.controllerMonitor.Device, out error);
            if (this.ProfileGenerated != null)
            {
                this.ProfileGenerated(this, new ProfileGeneratedEventArgs(midiProfile, error));
            }
        }

        private void OnMidiMessageReceived(object sender, MidiMessageEventArgs e)
        {
            this.OnPropertyChanged("NoteHistory");
            if (e.IsShortMessage)
            {
                byte controller = e.ShortMessage.Controller;
                byte velocity = e.ShortMessage.Velocity;
                bool isMSB = controller == 4;
                bool isLSB = controller == 36;

                if (this.controllerMonitor is ICC4HiHatPedalMonitor)
                {
                    if (isMSB || (isLSB && velocity == 0))
                    {
                        var pedalMonitor = this.controllerMonitor as ICC4HiHatPedalMonitor;
                        pedalMonitor.HiHatPedalVelocity = velocity;
                        return;
                    }
                }

                var buttons = this.ControllerMonitor.MonitorButtons.Where(
                    button => button.ProfileButton.Note == e.ShortMessage.Note &&
                    button.ProfileButton.Channel == e.ShortMessage.Channel);

                bool shouldHightlight = e.ShortMessage.StatusType != radio42.Multimedia.Midi.MIDIStatus.NoteOff;
                foreach (var button in buttons)
                {
                    this.ControllerMonitor.Highlight(button.ProfileButton.Name, shouldHightlight);
                }

                if (shouldHightlight && !this.WaitForNoteOff)
                {
                    var task = Task.Run(async delegate
                    {
                        await Task.Delay(MidiViewModel.UnhighlightDelayInMilliseconds);
                        foreach (var button in buttons)
                        {
                            this.ControllerMonitor.Highlight(button.ProfileButton.Name, false);
                        }
                    });
                }
            }
        }

        private void OnMonitorButtonCleared(object sender, EventArgs e)
        {
            var button = sender as MonitorButton;
            if (button != null)
            {
                this.controllerMonitor.Highlight(button.ProfileButton.Name, false);
            }
        }
    }
}