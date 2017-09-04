using Bmon.Client.Lib.Models;
using Bmon.Client.Lib.Transport;
using ManyConsole;
using System;
using System.Collections.Generic;

namespace Bmon.Client.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            Lib.Devour.DotCsv.GenericFormatA csv =
                new Lib.Devour.DotCsv.GenericFormatA(@"..\..\..\Bmon.Client.Lib.Devour.Tests\DotCsv\GenericFormatA.csv", Statics.ConfDebug);
            BmonTrendsForPost trends = csv.ParseTrends();

            Lib.Transport.Vendor.Bmon bmon = new Lib.Transport.Vendor.Bmon(new Uri("https://bmon.ahfc.us"));
            //bmon.PostAsync(trends);

            Console.ReadKey();

            var commands = GetCommands();
            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }

        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }
    }
}
