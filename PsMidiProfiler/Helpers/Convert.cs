namespace PsMidiProfiler.Helpers
{
    using System.Windows;

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
    }
}