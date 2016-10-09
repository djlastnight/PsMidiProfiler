using radio42.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsMidiProfiler
{
    public class NoteDetectedEventArgs : EventArgs
    {
        public  NoteDetectedEventArgs(int detectedNote, int channel, int noteOffValue)
        {
            this.DetectedNote = detectedNote;
            this.Channel = channel;
            this.NoteOffValue = noteOffValue;
        }

        public int DetectedNote
        {
            get;
            private set;
        }

        public int Channel
        {
            get;
            private set;
        }

        public int NoteOffValue
        {
            get;
            private set;
        }
    }
}
