using PsMidiProfiler.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PsMidiProfiler
{
    public class PsProfileButton : IXmlSerializable
    {
        private PsProfileButton()
        {
        }

        public PsProfileButton(ButtonName name, int note, int channel, int noteOffValue)
        {
            this.Name = name;
            this.Note = note;
            this.Channel = channel;
            this.NoteOffValue = noteOffValue;
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

            //var content = reader.ReadElementContentAsString();
            //string[] tokens = content.Split(new char[] { ',' });

            //this.Note = int.Parse(tokens[0]);
            //this.Channel = int.Parse(tokens[1]);
            //this.NoteOffValue = int.Parse(tokens[2]);
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