namespace PsMidiProfiler.Controls
{
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;

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
