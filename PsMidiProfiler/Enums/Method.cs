namespace PsMidiProfiler.Enums
{
    using System.ComponentModel;

    public enum Method
    {
        [Description("Method 0 - The default method. Listens for note on and note off events only.")]
        Manual = 0,

        [Description("Method 1 - Standard MIDI keyboard with pitch wheel bound to whammy. Buttons are not required.")]
        Piano = 1,

        [Description("Method 2 - Rock Band pro guitar, which is using system exclusive (SysEx) MIDI messages. Buttons are not required.")]
        GuitarSysEx = 2,

        [Description("Method 3 - Real drums with HiHat pedal, which reports its position as Control Change 4 MIDI Message. " + 
        "Phase Shift recognizes 3 hi-hat states depending on message velocity: Open (0-79), Sizzle (80-120), Closed (121-127).")]
        DrumsCC4HiHat = 3,

        [Description("Method 4 - Standard MIDI Guitar.")]
        GuitarMidi = 4,

        [Description("Method 5 - Legacy MIDI Guitar - Pitch wheel bound to whammy.")]
        GuitarLegacy = 5,
    }
}