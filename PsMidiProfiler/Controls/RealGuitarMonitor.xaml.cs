namespace PsMidiProfiler.Controls
{
    using System.Windows.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;

    /// <summary>
    /// Interaction logic for RealGuitarMonitor.xaml
    /// </summary>
    public partial class RealGuitarMonitor : UserControl, IControllerMonitor
    {
        private Controller controller;

        private PsDevice device;

        public RealGuitarMonitor()
        {
            this.InitializeComponent();
            this.controller = new Controller(ControllerType.RealGuitar, ControllerCategory.Guitars);
            this.device = new PsDevice("Sysex Based Real Guitar Midi Profile", DeviceType.GuitarReal);
            this.device.Method = (int)Method.SysEx;
        }

        public Controller Controller
        {
            get { return this.controller; }
        }

        public PsDevice Device
        {
            get { return this.device; }
        }
    }
}
