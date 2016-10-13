namespace PsMidiProfiler.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using PsMidiProfiler.Enums;
    using PsMidiProfiler.Models;

    public static class PsDeviceSerializer
    {
        public static string Serialize(PsDevice device, out string error)
        {
            if (device.ProfileButtons == null || device.ProfileButtons.Count == 0)
            {
                var strB = new StringBuilder();
                strB.AppendLine("<DEVICE>");
                strB.AppendFormat("\t<NAME>{0}</NAME>{1}", device.Name, Environment.NewLine);
                strB.AppendFormat("\t<DESCRIPTION>{0}</DESCRIPTION>{1}", device.Description, Environment.NewLine);
                strB.AppendFormat("\t<TYPE>{0}</TYPE>{1}", device.Type, Environment.NewLine);
                strB.AppendFormat("\t<METHOD>{0}</METHOD>{1}", device.Method, Environment.NewLine);
                strB.Append("</DEVICE>");
                error = null;
                return strB.ToString();
            }

            error = "Midi profile creation failed - not all buttons were set!";
            List<PsProfileButton> buttons = new List<PsProfileButton>();
            buttons.AddRange(device.ProfileButtons);

            // checking bass buttons
            var invalidBassButtons = buttons.Where(
                button => button.Name == ButtonName.Bass && button.Note == 0).ToList();

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

            // checking for not set buttons
            var invalidButtons = buttons.Where(button => button.Note == 0).ToList();
            if (invalidButtons.Count > 0)
            {
                return null;
            }

            // checking for note duplications
            var groups = buttons.GroupBy(
                button => button.Note,
                button => button.Name,
                (note, buttonNames) => new { DuplicatedNote = note, DuplicatedButtons = buttonNames.ToList() });

            var builder = new StringBuilder();

            foreach (var group in groups)
            {
                if (group.DuplicatedButtons.Count > 1)
                {
                    builder.AppendFormat("-Note {0} is set to multiply buttons:", group.DuplicatedNote);
                    foreach (var duplicatedButton in group.DuplicatedButtons)
                    {
                        builder.AppendFormat(" {0} ", duplicatedButton);
                    }

                    builder.AppendLine();
                }
            }

            if (!string.IsNullOrEmpty(builder.ToString()))
            {
                error = "Midi profile creation failed!\r\n\r\n";
                error += builder.ToString();
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

            string namespaceDefinition = " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"";
            sb.Replace(namespaceDefinition, string.Empty);
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

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlText), xSettings))
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                ws.IndentChars = "\t";
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