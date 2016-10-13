namespace PsMidiProfiler.Helpers
{
    using System;
    using System.Collections.Generic;
    using PsMidiProfiler.Controls;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;

    public static class ControllersHelper
    {
        public static List<Controller> GetControllers()
        {
            var controllers = new List<Controller>()
                {
                    new Controller(ControllerType.FourLaneDrums, ControllerCategory.Drums),
                    new Controller(ControllerType.FiveLaneDrums, ControllerCategory.Drums),
                    new Controller(ControllerType.SevenLaneDrums, ControllerCategory.Drums),
                    new Controller(ControllerType.RealDrums, ControllerCategory.Drums),
                    new Controller(ControllerType.RealDrumsCC4, ControllerCategory.Drums),
                    new Controller(ControllerType.FiveLaneKeys, ControllerCategory.Keys),
                    new Controller(ControllerType.TwoOctaveKeys, ControllerCategory.Keys),
                    new Controller(ControllerType.RealGuitar, ControllerCategory.Guitars)
                };

            return controllers;
        }

        internal static IControllerMonitor CreateMonitor(Controller controller)
        {
            IControllerMonitor monitor;
            switch (controller.Type)
            {
                case ControllerType.FourLaneDrums:
                    monitor = new FourLaneDrumsMonitor();
                    break;
                case ControllerType.FiveLaneDrums:
                    monitor = new FiveLaneDrumsMonitor();
                    break;
                case ControllerType.RealDrums:
                    monitor = new RealDrumsMonitor();
                    break;
                case ControllerType.RealDrumsCC4:
                    monitor = new RealDrumsCC4Monitor();
                    break;
                case ControllerType.SevenLaneDrums:
                    monitor = new SevenLaneDrumsMonitor();
                    break;
                case ControllerType.FiveLaneKeys:
                    monitor = new FiveLaneKeysMonitor();
                    break;
                case ControllerType.TwoOctaveKeys:
                    monitor = new TwoOctaveKeysMonitor();
                    break;
                case ControllerType.RealGuitar:
                    monitor = new RealGuitarMonitor();
                    break;
                default:
                    throw new NotImplementedException("Not implemented controller type: " + controller.Type);
            }

            return monitor;
        }
    }
}