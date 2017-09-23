using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bmon.Client.Core.Config.v1_0_0_0
{
    public class DevourConfig : IXmlSerializable
    {
        public List<Lib.Models.DevourModel> MyLocalFiles = new List<Lib.Models.DevourModel>();

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                if (reader.MoveToContent() == XmlNodeType.Element
                    && reader.LocalName == (typeof(Core.Config.v1_0_0_0.DevourConfig).Name)
                    && reader.GetAttribute("Version") == "1.0.0.0")
                {
                    reader.Read();

                    while (reader.MoveToContent() == XmlNodeType.Element
                        && reader.LocalName == (typeof(Lib.Models.DevourModel).Name))
                    {
                        var config = new XmlSerializer(typeof(Lib.Models.DevourModel));
                        MyLocalFiles.Add((Lib.Models.DevourModel)config.Deserialize(reader));
                    }
                }
                else
                    throw new InvalidDataException();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            try
            {
                writer.WriteAttributeString("Version", "1.0.0.0");

                foreach (var config in MyLocalFiles)
                {
                    var thing = new XmlSerializer(typeof(Lib.Models.DevourModel));
                    thing.Serialize(writer, config);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            foreach (var config in MyLocalFiles)
            {
                output.Append(typeof(Lib.Models.DevourModel).Name + Environment.NewLine);
                output.Append("  Id:" + config.Id.ToString() + Environment.NewLine);
                output.Append("  LocalDir:" + config.LocalDir.ToString() + Environment.NewLine);
                output.Append("  LocalFile:" + config.LocalFile.ToString() + Environment.NewLine);
                output.Append("  LocalFilePattern:" + config.LocalFilePattern.ToString() + Environment.NewLine);

                foreach (Guid g in config.UploadTo)
                    output.Append("    UploadTo:" + g.ToString() + Environment.NewLine);

                foreach (Guid g in config.TriggerOn)
                    output.Append("    TriggerOn:" + g.ToString() + Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
