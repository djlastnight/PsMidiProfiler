namespace PsMidiProfiler.Enums
{
    using System.ComponentModel;

    public enum DeviceType : int
    {
        [Description("Drums device type. Used for 4/5/7 lane MIDI drums. Combine with Method 0.")]
        Drums = 0,

        [Description("Guitar device type. Used for 5 fret MIDI guitars. Combine with Methods 0, 4 or 5")]
        Guitar = 1,

        [Description("Gamepad device type. Used to profile your MIDI device as gamepad. Combine with Method 0.")]
        Gamepad = 2,

        [Description("Computer keyboard device type.")]
        Keyboard = 3,

        [Description("Piano device type. The game supports 5-88 keys piano. It is recommended to use 5,13,25 or 88 keys piano. " +
            "Combine with method 1.")]
        Piano = 4,

        [Description("Real drums device type. Used for real drums with hi-hat pedal. Combine with Methods 0 or 3")]
        DrumsReal = 5,

        [Description("Real guitar device type. Used for Rock Band SysEx based real guitars. Combine with method 2.")]
        GuitarReal = 6,

        [Description("Dance pad device type. Used to profile dance pad controllers.")]
        DancePad = 7
    }
}