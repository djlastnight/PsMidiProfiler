namespace PsMidiProfiler.Models
{
    using System;
    using System.Xml.Serialization;
    using PsMidiProfiler.Enums;

    public class PsProfileButton : IXmlSerializable
    {
        public PsProfileButton(ButtonName name, int note, int channel, int noteOffValue)
        {
            this.Name = name;
            this.Note = note;
            this.Channel = channel;
            this.NoteOffValue = noteOffValue;
        }

        private PsProfileButton()
        {
        }

        public ButtonName Name { get; set; }

        public int Note { get; set; }

        public int Channel { get; set; }

        public int NoteOffValue { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0},{1},{2}",
                this.Note,
                this.Channel,
                this.NoteOffValue);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (writer.WriteState == System.Xml.WriteState.Element)
            {
                writer.WriteElementString(this.Name.ToString().ToUpper(), this.ToString());
            }
        }
    }
}