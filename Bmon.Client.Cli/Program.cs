using ManyConsole;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bmon.Client.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            var commands = GetCommands();
            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }

        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }
    }
}
