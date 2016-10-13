namespace PsMidiProfiler
{
    using System.Collections.Generic;
    using PsMidiProfiler.Controls;
    using PsMidiProfiler.Enums;

    public interface IButtonHighlighter
    {
        IEnumerable<MonitorButton> MonitorButtons { get; }

        void Highlight(ButtonName button, bool isNoteOn, byte velocity);
    }
}