using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Bmon.Client.Core.Config.v1_0_0_0
{
    public class UploadConfig : IXmlSerializable
    {
        public List<Lib.Models.FileViaFtpConfig> MyFtp = new List<Lib.Models.FileViaFtpConfig>();
        public List<Lib.Models.FileViaSftpConfig> MySftp = new List<Lib.Models.FileViaSftpConfig>();
        public List<Lib.Models.FileViaTftpConfig> MyTftp = new List<Lib.Models.FileViaTftpConfig>();
        public List<Lib.Models.PostFileToBmonConfig> MyPostFileToBmon = new List<Lib.Models.PostFileToBmonConfig>();
        public List<Lib.Models.PostFileToDropboxConfig> MyPostFileToDropbox = new List<Lib.Models.PostFileToDropboxConfig>();
        public List<Lib.Models.PostJsonToBmonConfig> MyPostJsonToBmon = new List<Lib.Models.PostJsonToBmonConfig>();

        public XmlSchema GetSchema() { return null; }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                if (reader.MoveToContent() == XmlNodeType.Element
                    && reader.LocalName == (typeof(Core.Config.v1_0_0_0.UploadConfig).Name)
                    && reader.GetAttribute("Version") == "1.0.0.0")
                {
                    reader.Read();

                    while (reader.MoveToContent() == XmlNodeType.Element
                        && reader.LocalName == (typeof(Lib.Models.FileViaFtpConfig).Name))
                    {
                        var config = new XmlSerializer(typeof(Lib.Models.FileViaFtpConfig));
                        MyFtp.Add((Lib.Models.FileViaFtpConfig)config.Deserialize(reader));
                    }

                    while (reader.MoveToContent() == XmlNodeType.Element
                        && reader.LocalName == (typeof(Lib.Models.FileViaSftpConfig).Name))
                    {
                        var config = new XmlSerializer(typeof(Lib.Models.FileViaSftpConfig));
                        MySftp.Add((Lib.Models.FileViaSftpConfig)config.Deserialize(reader));
                    }

                    while (reader.MoveToContent() == XmlNodeType.Element
                        && reader.LocalName == (typeof(Lib.Models.FileViaTftpConfig).Name))
                    {
                        var config = new XmlSerializer(typeof(Lib.Models.FileViaTftpConfig));
                        MyTftp.Add((Lib.Models.FileViaTftpConfig)config.Deserialize(reader));
                    }

                    while (reader.MoveToContent() == XmlNodeType.Element
                        && reader.LocalName == (typeof(Lib.Models.PostFileToBmonConfig).Name))
                    {
                        var config = new XmlSerializer(typeof(Lib.Models.PostFileToBmonConfig));
                        MyPostFileToBmon.Add((Lib.Models.PostFileToBmonConfig)config.Deserialize(reader));
                    }

                    while (reader.MoveToContent() == XmlNodeType.Element
                        && reader.LocalName == (typeof(Lib.Models.PostFileToDropboxConfig).Name))
                    {
                        var config = new XmlSerializer(typeof(Lib.Models.PostFileToDropboxConfig));
                        MyPostFileToDropbox.Add((Lib.Models.PostFileToDropboxConfig)config.Deserialize(reader));
                    }

                    while (reader.MoveToContent() == XmlNodeType.Element
                        && reader.LocalName == (typeof(Lib.Models.PostJsonToBmonConfig).Name))
                    {
                        var config = new XmlSerializer(typeof(Lib.Models.PostJsonToBmonConfig));
                        MyPostJsonToBmon.Add((Lib.Models.PostJsonToBmonConfig)config.Deserialize(reader));
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

                foreach (var config in MyFtp)
                {
                    var thing = new XmlSerializer(typeof(Lib.Models.FileViaFtpConfig));
                    thing.Serialize(writer, config);
                }

                foreach (var config in MySftp)
                {
                    var thing = new XmlSerializer(typeof(Lib.Models.FileViaSftpConfig));
                    thing.Serialize(writer, config);
                }

                foreach (var config in MyTftp)
                {
                    var thing = new XmlSerializer(typeof(Lib.Models.FileViaTftpConfig));
                    thing.Serialize(writer, config);
                }

                foreach (var config in MyPostFileToBmon)
                {
                    var thing = new XmlSerializer(typeof(Lib.Models.PostFileToBmonConfig));
                    thing.Serialize(writer, config);
                }

                foreach (var config in MyPostFileToDropbox)
                {
                    var thing = new XmlSerializer(typeof(Lib.Models.PostFileToDropboxConfig));
                    thing.Serialize(writer, config);
                }

                foreach (var config in MyPostJsonToBmon)
                {
                    var thing = new XmlSerializer(typeof(Lib.Models.PostJsonToBmonConfig));
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

            foreach (var config in MyFtp)
            {
                output.Append(typeof(Lib.Models.FileViaFtpConfig).Name + Environment.NewLine);
                output.Append("  Id:" + config.Id.ToString() + Environment.NewLine);
                output.Append("  Server:" + config.Server.ToString() + Environment.NewLine);
                output.Append("  Username:" + config.Credential.UserName.ToString() + Environment.NewLine);
                output.Append("  Password:" + config.Credential.Password.ToString() + Environment.NewLine);
                output.Append("  Path:" + config.Path.ToString() + Environment.NewLine);
            }

            output.Append(Environment.NewLine);

            foreach (var config in MySftp)
            {
                output.Append(typeof(Lib.Models.FileViaSftpConfig).Name + Environment.NewLine);
                output.Append("  Id:" + config.Id.ToString() + Environment.NewLine);
                output.Append("  Server:" + config.Server.ToString() + Environment.NewLine);
                output.Append("  Port:" + config.Port.ToString() + Environment.NewLine);
                output.Append("  Username:" + config.Credential.UserName.ToString() + Environment.NewLine);
                output.Append("  Password:" + config.Credential.Password.ToString() + Environment.NewLine);
                output.Append("  Path:" + config.Path.ToString() + Environment.NewLine);
            }

            output.Append(Environment.NewLine);

            foreach (var config in MyTftp)
            {
                output.Append(typeof(Lib.Models.FileViaTftpConfig).Name + Environment.NewLine);
                output.Append("  Id:" + config.Id.ToString() + Environment.NewLine);
                output.Append("  Server:" + config.Server.ToString() + Environment.NewLine);
                output.Append("  Path:" + config.Path.ToString() + Environment.NewLine);
            }

            output.Append(Environment.NewLine);

            foreach (var config in MyPostFileToBmon)
            {
                output.Append(typeof(Lib.Models.PostFileToBmonConfig).Name + Environment.NewLine);
                output.Append("  Id:" + config.Id.ToString() + Environment.NewLine);
                output.Append("  Server:" + config.Server.ToString() + Environment.NewLine);
                output.Append("  Path:" + config.Path.ToString() + Environment.NewLine);
                output.Append("  LocalDir:" + config.LocalDir.ToString() + Environment.NewLine);
                output.Append("  LocalFile:" + config.LocalFile.ToString() + Environment.NewLine);
            }

            output.Append(Environment.NewLine);

            foreach (var config in MyPostFileToDropbox)
            {
                output.Append(typeof(Lib.Models.PostFileToDropboxConfig).Name + Environment.NewLine);
                output.Append("  Id:" + config.Id.ToString() + Environment.NewLine);
                output.Append("  Token:" + config.Token.ToString() + Environment.NewLine);
                output.Append("  Path:" + config.Path.ToString() + Environment.NewLine);
            }

            output.Append(Environment.NewLine);

            foreach (var config in MyPostJsonToBmon)
            {
                output.Append(typeof(Lib.Models.PostJsonToBmonConfig).Name + Environment.NewLine);
                output.Append("  Id:" + config.Id.ToString() + Environment.NewLine);
                output.Append("  Server:" + config.Server.ToString() + Environment.NewLine);
                output.Append("  Path:" + config.Path.ToString() + Environment.NewLine);
                output.Append("  StoreKey:" + config.StoreKey.ToString() + Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
