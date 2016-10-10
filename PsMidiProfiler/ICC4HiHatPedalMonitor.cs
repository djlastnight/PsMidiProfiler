using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PsMidiProfiler
{
    public interface ICC4HiHatPedalMonitor
    {
        byte HiHatPedalVelocity { get; set; }
    }
}
