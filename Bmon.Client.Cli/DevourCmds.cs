using Bmon.Client.Lib.Devour;
using Bmon.Client.Lib.Models;
using ManyConsole;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bmon.Client.Cli
{
    public class DevourCmds : ConsoleCommand
    {
        private OutputFormat Format;
        private string InputFile = null;
        private bool Validate = false, Show = false;

        public DevourCmds()
        {
            IsCommand("devour", "Do things with a trend file...");

            HasRequiredOption("f|file=", "File to parse. File must exist.", arg =>
            {
                Helpers.FileSanityChecks(ref arg, ref InputFile);
            });
            HasOption("s|show", "Show parsed information.", arg => { Show = true; });
            HasOption("o|output-format=", "Show output format.", arg => 
            {
                if (Show)
                {
                    if (arg.ToLower() == OutputFormat.Csv.ToString().ToLower())
                        Format = OutputFormat.Csv;

                    else if (arg.ToLower() == OutputFormat.Json.ToString().ToLower())
                        Format = OutputFormat.Json;

                    else
                        throw new ConsoleHelpAsException("Invalid output format...");
                }
                else
                    throw new ConsoleHelpAsException("Invalid use of output-format...");
            });
            HasOption("v|validate", "Validate file.", arg => { Validate = true; });
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                Lib.Devour.DotCsv.GenericFormatA raw = new Lib.Devour.DotCsv.GenericFormatA(InputFile);
                MomentTuples momentTuples = new MomentTuples();
                MomentArrays momentArrays = new MomentArrays();

                if (Show)
                {
                    raw.Parse(ref momentTuples);
                    Console.WriteLine(raw.Output);

                    foreach (Tuple<double, string, double> t in momentTuples.Readings)
                        momentArrays.Readings.Add(new List<string>() { t.Item1.ToString(), t.Item2.ToString(), t.Item3.ToString() });

                    switch (Format)
                    {
                        case OutputFormat.Csv:
                            {
                                Console.WriteLine(momentTuples.ToString());
                            }
                            break;

                        case OutputFormat.Json:
                            {
                                Console.WriteLine(JsonConvert.SerializeObject(momentArrays, Formatting.Indented));
                            }
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                }

                if (Validate)
                {
                    raw.Debug = false;

                    if (raw.TryParse(ref momentTuples))
                        Console.WriteLine("File has a valid format.");
                    else
                        Console.WriteLine("File has an invalid format.");
                }

                return Helpers.FondFarewell();
            }
            catch (Exception ex)
            {
                Core.Echo.Proxy.Caught.Msg(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return Helpers.AngryFarewell(ex);
            }
        }
    }
}
