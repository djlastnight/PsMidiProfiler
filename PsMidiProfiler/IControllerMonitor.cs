namespace PsMidiProfiler
{
    using System.Collections.Generic;
    using PsMidiProfiler.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;

    public interface IControllerMonitor
    {
        Controller Controller { get; }

        PsDevice Device { get; }

        IEnumerable<MonitorButton> MonitorButtons { get; }

        void Highlight(ButtonName button, bool value);
    }
}
