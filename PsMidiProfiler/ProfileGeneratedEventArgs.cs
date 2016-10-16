namespace PsMidiProfiler
{
    using System;
    using PsMidiProfiler.Models;

    public class ProfileGeneratedEventArgs : EventArgs
    {
        private readonly MidiProfile profile;

        public ProfileGeneratedEventArgs(MidiProfile profile)
        {
            this.profile = profile;
        }

        public MidiProfile Profile
        {
            get
            {
                return this.profile;
            }
        }
    }
}