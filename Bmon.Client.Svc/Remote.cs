using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security.Permissions;
using System.Text;
// http://msdn2.microsoft.com/en-us/library/system.runtime.remoting.channels.tcp.tcpchannel(VS.80).aspx

namespace Bmon.Client.Svc
{
    public class Control
    {
        [SecurityPermission(SecurityAction.Demand)]
        public static void Initialize()
        {
            try
            {
                StringBuilder rslt = new StringBuilder();

                //create the server channel.
                //IDictionary props = new Hashtable();
                //props["bindto"] = "127.0.0.1";
                //props["port"] = "666";
                //TcpChannel serverChannel = new TcpChannel(props, null, new BinaryServerFormatterSinkProvider());
                TcpChannel serverChannel = new TcpChannel(666);

                //register the server channel.
                ChannelServices.RegisterChannel(serverChannel, true);

                //show the _id of the channel.
                rslt.Append(Environment.NewLine + "channel type is " + serverChannel.ChannelName);

                //show the priority of the channel.
                rslt.Append(Environment.NewLine + "channel priority is " + serverChannel.ChannelPriority);

                //show the URIs associated with the channel.
                ChannelDataStore data = (ChannelDataStore)serverChannel.ChannelData;
                foreach (string uri in data.ChannelUris)
                {
                    rslt.Append(Environment.NewLine + "channel uri is " + uri);
                }

                //expose an object for remote calls.
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(Bmon.Client.Svc.Proxy.Remote), "RemoteControlObject", WellKnownObjectMode.Singleton);

                //parse the channel's URI.
                string[] urls = serverChannel.GetUrlsForUri("RemoteControlObject");

                if (urls.Length > 0)
                {
                    string objectUrl = urls[0];
                    string objectUri;
                    string channelUri = serverChannel.Parse(objectUrl, out objectUri);
                    rslt.Append(Environment.NewLine + "object URL is " + objectUrl);
                    rslt.Append(Environment.NewLine + "object URI is " + objectUri);
                    rslt.Append(Environment.NewLine + "channel URI is " + channelUri);
                }

                Bmon.Client.Core.Echo.Proxy.Audit.Msg(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().ToString(), rslt, Bmon.Client.Core.Echo.Proxy.levels.debug);
            }
            catch (Exception ex)
            {
                Bmon.Client.Core.Echo.Proxy.Caught.Msg(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
