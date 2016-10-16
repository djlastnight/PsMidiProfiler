namespace PsMidiProfiler.Models
{
    using PsMidiProfiler.Enums;

    public class MidiProfile
    {
        public MidiProfile(string profileText, string errorText, MidiProfileErrorType errorType)
        {
            this.ProfileText = profileText;
            this.ErrorText = errorText;
            this.ErrorType = errorType;
        }

        public string ProfileText { get; private set; }

        public string ErrorText { get; private set; }

        public MidiProfileErrorType ErrorType { get; private set; }
    }
}