using PsMidiProfiler.Controls;
using PsMidiProfiler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsMidiProfiler
{
    public interface IControllerMonitor
    {
        ControllerType ControllerType { get; }

        PsDevice Device { get; }

        IEnumerable<MonitorButton> MonitorButtons { get; }

        void Highlight(ButtonName button, bool value);
    }
}
