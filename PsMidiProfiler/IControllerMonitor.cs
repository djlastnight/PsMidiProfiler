namespace PsMidiProfiler
{
    using System.Collections.Generic;
    using PsMidiProfiler.Controls;
    using PsMidiProfiler.Enums;

    public interface IControllerMonitor
    {
        ControllerType ControllerType { get; }

        PsDevice Device { get; }

        IEnumerable<MonitorButton> MonitorButtons { get; }

        void Highlight(ButtonName button, bool value);
    }
}
