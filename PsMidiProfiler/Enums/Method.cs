using System.ComponentModel;
namespace PsMidiProfiler.Enums
{
    public enum Method
    {
        [Description("The default method. Listens for note on and note off events only.")]
        Manual = 0,

        [Description("Standard MIDI keyboard, which pitch wheel is bound to whammy")]
        Piano = 1,

        [Description("Rock Band pro guitar, which is using system exclusive (SysEx) MIDI messages.")]
        GuitarSysEx = 2,

        [Description("Real drums with HiHat pedal, which reports its state as midi Control Change 4 (36) MIDI Message")]
        DrumsCC4HiHat = 3,

        [Description("Standard MIDI Guitar")]
        GuitarMidi = 4,

        [Description("MIDI Guitar, which pitch wheel is bound to whammy")]
        GuitarLegacy = 5,
    }
}