namespace PsMidiProfiler.Helpers
{
    using System.Windows;
    using PsMidiProfiler.Enums;

    public static class Convert
    {
        public static Visibility ToVisibility(bool value)
        {
            if (value)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public static HiHatState ToHiHatState(byte velocity)
        {
            if (velocity >= 120)
            {
                return HiHatState.Closed;
            }
            else if (velocity >= 80)
            {
                return HiHatState.HalfClosed;
            }
            else
            {
                return HiHatState.Opened;
            }
        }
    }
}