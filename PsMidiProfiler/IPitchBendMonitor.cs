namespace PsMidiProfiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPitchBendMonitor
    {
        float PitchWheelValue { get; set; }
    }
}