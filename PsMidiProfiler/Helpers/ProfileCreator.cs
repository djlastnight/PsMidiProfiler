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

    public static class ProfileCreator
    {
        public static MidiProfile Create(IControllerMonitor monitor, bool checkForZeroOrNullButtons)
        {
            if (monitor == null)
            {
                throw new ArgumentNullException("monitor");
            }

            PsDevice device = monitor.Device;

            if ((monitor is IButtonHighlighter) == false)
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendLine("<DEVICE>");
                strBuilder.AppendFormat("\t<NAME>{0}</NAME>{1}", device.Name, Environment.NewLine);
                strBuilder.AppendFormat("\t<DESCRIPTION>{0}</DESCRIPTION>{1}", device.Description, Environment.NewLine);
                strBuilder.AppendFormat("\t<TYPE>{0}</TYPE>{1}", device.Type, Environment.NewLine);
                if (device.Method != 0)
                {
                    strBuilder.AppendFormat("\t<METHOD>{0}</METHOD>{1}", device.Method, Environment.NewLine);
                }

                strBuilder.Append("</DEVICE>");

                return new MidiProfile(
                    strBuilder.ToString(),
                    null,
                    MidiProfileErrorType.NoError);
            }

            bool buttonsAreInvalid = device.ProfileButtons == null || device.ProfileButtons.Count == 0;

            if (checkForZeroOrNullButtons && buttonsAreInvalid)
            {
                return new MidiProfile(
                    null,
                    "Midi profile creation failed - no buttons!",
                    MidiProfileErrorType.NoButtonsDefined);
            }

            if (device.ProfileButtons.Where(button => button.Name == ButtonName.None).Count() > 0)
            {
                return new MidiProfile(
                    null,
                    "Midi profile creation failed - button name can not be 'None'!",
                    MidiProfileErrorType.NoneButtonNameDetected);
            }

            string error = "Midi profile creation failed - not all notes were set!";

            var buttons = new List<PsProfileButton>(device.ProfileButtons);

            // checking bass buttons
            var invalidBassButtons = buttons.Where(
                button => button.Name == ButtonName.Bass && button.Note == 0).ToList();

            if (invalidBassButtons.Count == 2)
            {
                return new MidiProfile(null, error, MidiProfileErrorType.ZeroNoteDetected);
            }
            else if (invalidBassButtons.Count == 1)
            {
                // allowing to set just one bass button. Removing the invalid one.
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
                    // both bass buttons has the same note. Removing this one, which has higher note off value.
                    var bassWithHigherNoteOffValue = bassButtons.OrderBy(x => x.NoteOffValue).Last();
                    buttons.Remove(bassWithHigherNoteOffValue);
                }
            }

            // checking for not set buttons
            var invalidButtons = buttons.Where(button => button.Note == 0).ToList();
            if (invalidButtons.Count > 0)
            {
                return new MidiProfile(null, error, MidiProfileErrorType.ZeroNoteDetected);
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

                return new MidiProfile(null, error, MidiProfileErrorType.NoteDuplicationsDetected);
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

            sb.Replace("<ProfileButton>", string.Empty);
            sb.Replace("</ProfileButton>", string.Empty);

            if (device.Method == 0)
            {
                sb.Replace("<METHOD>0</METHOD>", string.Empty);
            }

            if (device.ProfileButtons.Count == 0)
            {
                sb.Replace("<BUTTONS />", string.Empty);
            }

            var formatted = ProfileCreator.AutoFormatXml(sb.ToString());

            return new MidiProfile(formatted, null, MidiProfileErrorType.NoError);
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