using radio42.Multimedia.Midi;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PsMidiProfiler
{
    public class MidiModel
    {
        private readonly bool autoClearHistory;

        private MidiInputDevice midiIn;

        private List<string> midiInDevices;

        private string currentMidiInDevice;

        private StringBuilder noteHistory;

        /// <summary>
        /// Used to avoid the StringBuilder cross-thread issue.
        /// </summary>
        private object lockObject = new object();

        public MidiModel(bool autoclearHistory = true)
        {
            this.autoClearHistory = autoclearHistory;
            this.noteHistory = new StringBuilder(20000);

            this.AddMessageToMonitor(
                string.Format("Application version {0}", this.GetAssemblyVersion()));

            this.RefreshDevices();
        }

        private string GetAssemblyVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;

            return version;
        }

        public IEnumerable<string> MidiInDevices
        {
            get
            {
                return this.midiInDevices;
            }
        }

        public string CurrentDevice
        {
            get
            {
                return this.currentMidiInDevice;
            }

            set
            {
                this.SetCurrentDevice(value);
            }
        }

        public string NoteHistory
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.noteHistory.ToString();
                }
            }
        }

        public event MidiMessageEventHandler MessageReceived;

        public void RefreshDevices()
        {
            if (MidiInputDevice.GetDeviceCount() == 0)
            {
                return;
            }

            this.midiInDevices = MidiInputDevice.GetDeviceDescriptions().ToList();
        }

        public void ClearNoteHistory()
        {
            lock (this.lockObject)
            {
                this.noteHistory.Clear();
            }
        }

        private void SetCurrentDevice(string value)
        {
            if (this.currentMidiInDevice != null)
            {
                this.midiIn.Stop();
                this.midiIn.Close();
                this.midiIn.MessageReceived -= this.OnMidiMessageReceived;
            }

            this.currentMidiInDevice = value;
            int index = this.midiInDevices.IndexOf(value);
            this.midiIn = new MidiInputDevice(index);
            this.midiIn.AutoPairController = true;
            this.midiIn.MessageFilter = MIDIMessageType.SystemRealtime | MIDIMessageType.SystemExclusive;
            this.midiIn.MessageReceived += OnMidiMessageReceived;

            string error = null;
            if (this.midiIn.Open())
            {
                if (!this.midiIn.Start())
                {
                    error = this.midiIn.LastErrorCode.ToString();
                }
            }
            else
            {
                error = this.midiIn.LastErrorCode.ToString();
            }

            if (error != null)
            {
                this.AddMessageToMonitor("Midi device could not be started! Error: " + error);
            }
        }

        private void OnMidiMessageReceived(object sender, MidiMessageEventArgs e)
        {
            if (this.MessageReceived != null)
            {
                this.MessageReceived(sender, e);
            }

            this.AddMessageToMonitor(e);
        }

        private void AddMessageToMonitor(MidiMessageEventArgs e)
        {
            string message = null;

            if (e.IsShortMessage)
            {
                message = string.Format(
                    "[{0:D2}:{1:D2}:{2:D3}] - {3} | {4} | Channel: {5} | Velocity: {6}",
                    e.ShortMessage.Timespan.Minutes,
                    e.ShortMessage.Timespan.Seconds,
                    e.ShortMessage.Timespan.Milliseconds,
                    e.ShortMessage.StatusType,
                    e.ShortMessage.Note,
                    e.ShortMessage.Channel,
                    e.ShortMessage.Velocity);
            }
            else if (e.IsSysExMessage)
            {
                message = e.SysExMessage.ToString();
            }
            else if (e.EventType == radio42.Multimedia.Midi.MidiMessageEventType.Opened)
            {
                message = string.Format("Midi device {0} opened.", e.DeviceID);
            }
            else if (e.EventType == radio42.Multimedia.Midi.MidiMessageEventType.Closed)
            {
                message = string.Format("Midi device {0} closed.", e.DeviceID);
            }
            else if (e.EventType == radio42.Multimedia.Midi.MidiMessageEventType.Started)
            {
                message = string.Format("Midi device {0} started.", e.DeviceID);
            }
            else if (e.EventType == radio42.Multimedia.Midi.MidiMessageEventType.Stopped)
            {
                message = string.Format("Midi device {0} stopped.", e.DeviceID);
            }

            this.AddMessageToMonitor(message);
        }

        private void AddMessageToMonitor(string message)
        {
            lock (this.lockObject)
            {
                if (this.autoClearHistory && this.noteHistory.Length > 15000)
                {
                    this.noteHistory.Clear();
                    this.noteHistory.AppendLine("-- auto cleared --");
                }

                this.noteHistory.AppendLine(message);
            }
        }
    }
}