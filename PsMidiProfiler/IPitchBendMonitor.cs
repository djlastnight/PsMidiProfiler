using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsMidiProfiler
{
    public interface IPitchBendMonitor
    {
        float PitchWheelValue { get; set; }
    }
}
