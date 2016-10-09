namespace PsMidiProfiler
{
    using System;

    public class ProfileGeneratedEventArgs : EventArgs
    {
        private readonly string profileText;

        private readonly string error;

        public ProfileGeneratedEventArgs(string profileText, string error)
        {
            this.profileText = profileText;
            this.error = error;
        }

        public string ProfileText
        {
            get
            {
                return this.profileText;
            }
        }

        public string Error
        {
            get
            {
                return this.error;
            }
        }
    }
}