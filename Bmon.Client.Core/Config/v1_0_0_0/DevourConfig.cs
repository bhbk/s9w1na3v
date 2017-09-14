using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bmon.Client.Core.Config.v1_0_0_0
{
    public class DevourConfig : IXmlSerializable
    {
        public List<Lib.Models.DevourConfig> MyFiles = new List<Lib.Models.DevourConfig>();

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {

        }

        public void WriteXml(XmlWriter writer)
        {

        }
    }
}
