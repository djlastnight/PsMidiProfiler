using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PsMidiProfiler
{
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
