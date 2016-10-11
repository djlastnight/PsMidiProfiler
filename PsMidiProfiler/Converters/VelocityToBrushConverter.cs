namespace PsMidiProfiler.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class VelocityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush))
            {
                return null;
            }

            byte input;
            if (byte.TryParse(value.ToString(), out input))
            {
                var brush = new SolidColorBrush();
                var hihatState = Helpers.Convert.ToHiHatState(input);
                if (hihatState == Enums.HiHatState.Closed)
                {
                    brush.Color = Colors.LightGreen;
                }
                else if (hihatState == Enums.HiHatState.HalfClosed)
                {
                    brush.Color = Colors.Red;
                }
                else
                {
                    brush.Color = Colors.Yellow;
                }

                return brush;
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
