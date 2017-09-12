using Bmon.Client.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bmon.Client.Core.Config
{
    public class v1_0_0_0
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

        public class UploadConfig : IXmlSerializable
        {
            public List<Lib.Models.FileToDropboxConfig> MyDropbox = new List<Lib.Models.FileToDropboxConfig>();
            public List<Lib.Models.FileViaFtpConfig> MyFtp = new List<Lib.Models.FileViaFtpConfig>();
            public List<Lib.Models.FileViaSftpConfig> MySftp = new List<Lib.Models.FileViaSftpConfig>();
            public List<Lib.Models.FileViaTftpConfig> MyTftp = new List<Lib.Models.FileViaTftpConfig>();
            public List<Lib.Models.WebApiToBmonConfig> MyWebApiToBmon = new List<Lib.Models.WebApiToBmonConfig>();

            public XmlSchema GetSchema() { return null; }

            public void ReadXml(XmlReader reader)
            {

            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteAttributeString("Version", "1.0.0.0");

                foreach (var config in MyDropbox)
                {
                    var other = new XmlSerializer(typeof(Lib.Models.FileToDropboxConfig));
                    other.Serialize(writer, config);
                }

                foreach (var config in MyFtp)
                {
                    var other = new XmlSerializer(typeof(Lib.Models.FileViaFtpConfig));
                    other.Serialize(writer, config);
                }

                foreach (var config in MySftp)
                {
                    var other = new XmlSerializer(typeof(Lib.Models.FileViaSftpConfig));
                    other.Serialize(writer, config);
                }

                foreach (var config in MyTftp)
                {
                    var other = new XmlSerializer(typeof(Lib.Models.FileViaTftpConfig));
                    other.Serialize(writer, config);
                }

                foreach (var config in MyWebApiToBmon)
                {
                    var other = new XmlSerializer(typeof(Lib.Models.WebApiToBmonConfig));
                    other.Serialize(writer, config);
                }
            }
        }
    }
}
