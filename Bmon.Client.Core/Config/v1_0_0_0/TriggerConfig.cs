using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bmon.Client.Core.Config.v1_0_0_0
{
    public class TriggerConfig : IXmlSerializable
    {
        public List<Lib.Models.TriggerModel> MyTriggers = new List<Lib.Models.TriggerModel>();

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                if (reader.MoveToContent() == XmlNodeType.Element
                    && reader.LocalName == (typeof(Core.Config.v1_0_0_0.TriggerConfig).Name)
                    && reader.GetAttribute("Version") == "1.0.0.0")
                {
                    reader.Read();

                    while (reader.MoveToContent() == XmlNodeType.Element
                        && reader.LocalName == (typeof(Lib.Models.TriggerModel).Name))
                    {
                        var config = new XmlSerializer(typeof(Lib.Models.TriggerModel));
                        MyTriggers.Add((Lib.Models.TriggerModel)config.Deserialize(reader));
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

                foreach (var config in MyTriggers)
                {
                    var thing = new XmlSerializer(typeof(Lib.Models.TriggerModel));
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

            foreach (var config in MyTriggers)
            {
                output.Append(typeof(Lib.Models.TriggerModel).Name + Environment.NewLine);
                output.Append("  Id:" + config.Id.ToString() + Environment.NewLine);
                output.Append("  LocalDir:" + config.LocalDir.ToString() + Environment.NewLine);
                output.Append("  LocalFile:" + config.LocalFile.ToString() + Environment.NewLine);
                output.Append("  LocalFilePattern:" + config.LocalFileTrigger.ToString() + Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
