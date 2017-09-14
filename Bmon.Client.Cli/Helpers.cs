using Bmon.Client.Lib.Models;
using ManyConsole;
using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace Bmon.Client.Cli
{
    internal class Helpers
    {
        internal static void FileSanityChecks(ref string arg, ref string file)
        {
            if (arg == string.Empty)
                throw new ConsoleHelpAsException(string.Format("No file name was given.", arg));

            else if (!File.Exists(arg))
                throw new ConsoleHelpAsException(string.Format("The file {0} does not exist.", arg));

            else
                file = arg;
        }

        internal static int FondFarewell()
        {
            //Console.ReadKey();
            return (int)ExitCodes.Success;
        }

        internal static int AngryFarewell(Exception ex)
        {
            //Core.Echo.Proxy.Caught.Msg(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().ToString(), ex);
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);

            //Console.ReadKey();
            return (int)ExitCodes.Exception;
        }

        internal static void ReadConfig(ref Core.Config.v1_0_0_0.UploadConfig config)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Core.Config.v1_0_0_0.UploadConfig));
            StreamReader sr = new StreamReader(Core.Config.Globals.UploadConfigFile);
            config = (Core.Config.v1_0_0_0.UploadConfig)xs.Deserialize(sr);
        }

        internal static void WriteConfig(ref Core.Config.v1_0_0_0.UploadConfig config)
        {
            XmlSerializer xs = new XmlSerializer(config.GetType());
            StreamWriter sw = new StreamWriter(Core.Config.Globals.UploadConfigFile);
            xs.Serialize(sw, config);
        }
        
        internal static void InitializeConfig(ref Core.Config.v1_0_0_0.UploadConfig config)
        {
            config.MyDropbox.Add(new FileToDropboxConfig("12345678", "/"));
            config.MyFtp.Add(new FileViaFtpConfig("ftp://bmon.ahfc.us", new NetworkCredential("username", "password"), "/"));
            config.MySftp.Add(new FileViaSftpConfig("sftp://bmon.ahfc.us", 22, new NetworkCredential("username", "password"), "/"));
            config.MyTftp.Add(new FileViaTftpConfig("tftp://bmon.ahfc.us", "/"));
            config.MyWebApiToBmon.Add(new WebApiToBmonConfig("https://bmon.ahfc.us", "/readingdb/reading/store/", "12345678"));

            WriteConfig(ref config);
        }
    }
}
