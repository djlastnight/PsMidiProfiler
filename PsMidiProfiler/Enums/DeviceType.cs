using System.ComponentModel;
namespace PsMidiProfiler.Enums
{
    public enum DeviceType : int
    {
        [Description("Drums device type. Used for 4/5/7 lane drums and real drums with hi-hat pedal (non CC4).")]
        Drums = 0,

        [Description("Guitar device type. Used for 5 fret guitars.")]
        Guitar = 1,

        [Description("Gamepad device type. Used to profile xInput and direct input gamepads.")]
        Gamepad = 2,

        [Description("Computer keyboard device type.")]
        Keyboard = 3,

        [Description("Piano device type. The game supports 5-88 keys piano. It is recommended to use 5,13,25 or 88 keys.")]
        Piano = 4,

        [Description("Real drums device type. Used for real drums with CC4 hi-hat pedal support.")]
        DrumsReal = 5,

        [Description("Real guitar device type. Used for Rock Band SysEx based real guitars.")]
        GuitarReal = 6,

        [Description("Dance pad device type. Used to profile dance pad controllers.")]
        DancePad = 7
    }
}