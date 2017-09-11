using Bmon.Client.Lib.Models;
using System;
using System.Collections.Generic;
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
            public List<Lib.Models.BaseUploadConfig.FileToDropboxConfig> MyDropbox = new List<Lib.Models.BaseUploadConfig.FileToDropboxConfig>();
            public List<Lib.Models.BaseUploadConfig.FileViaFtpConfig> MyFtp = new List<Lib.Models.BaseUploadConfig.FileViaFtpConfig>();
            public List<Lib.Models.BaseUploadConfig.FileViaSftpConfig> MySftp = new List<Lib.Models.BaseUploadConfig.FileViaSftpConfig>();
            public List<Lib.Models.BaseUploadConfig.FileViaTftpConfig> MyTftp = new List<Lib.Models.BaseUploadConfig.FileViaTftpConfig>();
            public List<Lib.Models.BaseUploadConfig.WebApiToBmonConfig> MyBmon = new List<Lib.Models.BaseUploadConfig.WebApiToBmonConfig>();

            public XmlSchema GetSchema() { return null; }

            public void ReadXml(XmlReader reader)
            {

            }

            public void WriteXml(XmlWriter writer)
            {

            }
        }
    }
}
