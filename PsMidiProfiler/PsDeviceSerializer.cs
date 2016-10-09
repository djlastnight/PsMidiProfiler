using PsMidiProfiler.Enums;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

namespace PsMidiProfiler
{
    public static class PsDeviceSerializer
    {
        public static string Serialize(PsDevice device, out string error)
        {
            if (device.ProfileButtons == null || device.ProfileButtons.Count == 0)
            {
                error = "Midi profile creation failed - no buttons data";
                return null;
            }

            error = "Midi profile creation failed - not all buttons were set!";
            List<PsProfileButton> buttons = new List<PsProfileButton>();
            buttons.AddRange(device.ProfileButtons);

            var invalidBassButtons = buttons.Where(button => button.Name == ButtonName.Bass && button.Note == 0).ToList();
            if (invalidBassButtons.Count == 2)
            {
                return null;
            }
            else if (invalidBassButtons.Count == 1)
            {
                buttons.Remove(invalidBassButtons[0]);
            }

            var bassButtons = buttons.Where(button => button.Name == ButtonName.Bass).ToList();
            if (bassButtons.Count == 2)
            {
                var bass1 = bassButtons[0];
                var bass2 = bassButtons[1];

                if (bass1.Note == bass2.Note &&
                    bass1.Channel == bass2.Channel)
                {
                    var bassWithHigherNoteOffValue = bassButtons.OrderBy(x => x.NoteOffValue).Last();
                    buttons.Remove(bassWithHigherNoteOffValue);
                }
            }

            var invalidButtons = buttons.Where(button => button.Note == 0).ToList();
            if (invalidButtons.Count > 0)
            {
                return null;
            }

            var deviceToSerialize = new PsDevice();
            deviceToSerialize.Description = device.Description;
            deviceToSerialize.ProfileButtons = buttons;
            deviceToSerialize.Method = device.Method;
            deviceToSerialize.Name = device.Name;
            deviceToSerialize.Type = device.Type;

            XmlSerializer serializer = new XmlSerializer(typeof(PsDevice));
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xSettings = new XmlWriterSettings()
            {
                IndentChars = "\t",
                Indent = true,
                Encoding = Encoding.Unicode,
                OmitXmlDeclaration = true,
            };

            using (XmlWriter writer = XmlWriter.Create(sb, xSettings))
            {
                serializer.Serialize(writer, deviceToSerialize);
            }

            string nsDef = " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";
            sb.Replace(nsDef, string.Empty);
            sb.Replace("<ProfileButton>", string.Empty);
            sb.Replace("</ProfileButton>", string.Empty);
            var formatted = PsDeviceSerializer.AutoFormatXml(sb.ToString());

            error = null;
            return formatted;
        }

        private static string AutoFormatXml(string xmlText)
        {
            StringBuilder output = new StringBuilder();
            XmlReaderSettings xSettings = new XmlReaderSettings();
            xSettings.ConformanceLevel = ConformanceLevel.Fragment;
            // Create an XmlReader
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlText), xSettings))
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                ws.IndentChars = "\t";
                //ws.CheckCharacters = true;
                ws.ConformanceLevel = ConformanceLevel.Fragment;
                using (XmlWriter writer = XmlWriter.Create(output, ws))
                {
                    // Parse the text and display each of the nodes.
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                writer.WriteStartElement(reader.Name);
                                break;
                            case XmlNodeType.Text:
                                writer.WriteString(reader.Value);
                                break;
                            case XmlNodeType.Comment:
                                writer.WriteComment(reader.Value);
                                break;
                            case XmlNodeType.EndElement:
                                writer.WriteFullEndElement();
                                break;
                        }
                    }
                }
            }

            return output.ToString();
        }
    }
}