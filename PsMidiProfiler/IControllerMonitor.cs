namespace PsMidiProfiler
{
    using PsMidiProfiler.Models;

    public interface IControllerMonitor
    {
        //Controller Controller { get; }

        PsDevice Device { get; }
    }
}
