namespace PsMidiProfiler.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using PsMidiProfiler.Enums;

    [Serializable]
    [XmlRoot("DEVICE")]
    public class PsDevice
    {
        public PsDevice()
        {
            this.Name = "Name not set";
            this.Description = "Generated using Phase Shift Midi Profiler by djlastnight";
            this.Method = 1;
            this.Type = 0;
        }

        public PsDevice(string name, DeviceType type)
            : this()
        {
            this.Name = name;
            this.Type = (int)type;
        }

        [XmlElement("NAME")]
        public string Name { get; set; }

        [XmlElement("DESCRIPTION")]
        public string Description { get; set; }

        [XmlElement("TYPE")]
        public int Type { get; set; }

        [XmlElement("METHOD")]
        public int Method { get; set; }

        [XmlArray("BUTTONS")]
        [XmlArrayItem("ProfileButton")]
        public List<PsProfileButton> ProfileButtons { get; set; }
    }
}
