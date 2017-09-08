using Bmon.Client.Lib.Devour;
using ManyConsole;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bmon.Client.Cli
{
    public class DevourCmds : ConsoleCommand
    {
        private OutputFormat format;
        private string file = null;
        private bool validate = false, show = false;

        public DevourCmds()
        {
            IsCommand("devour", "Do things with a trend file...");

            HasRequiredOption("f|file=", "File to parse. File must exist.", arg =>
            {
                Helpers.FileSanityChecks(ref arg, ref file);
            });
            HasOption("s|show", "Show parsed information.", arg => { show = true; });
            HasOption("o|output-format=", "Show output format.", arg => 
            {
                if (show)
                {
                    if (arg == OutputFormat.Csv.ToString())
                        format = OutputFormat.Csv;

                    else if (arg == OutputFormat.Json.ToString())
                        format = OutputFormat.Json;

                    else
                        throw new ConsoleHelpAsException("Invalid output format...");
                }
                else
                    throw new ConsoleHelpAsException("Invalid use of output-format...");
            });
            HasOption("v|validate", "Validate file.", arg => { validate = true; });
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                Lib.Devour.DotCsv.GenericFormatA raw = new Lib.Devour.DotCsv.GenericFormatA(file);
                Lib.Models.MultipleMomentsTuples momentsTuples = new Lib.Models.MultipleMomentsTuples();
                Lib.Models.MultipleMomentsArrays momentsArrays = new Lib.Models.MultipleMomentsArrays();

                if (show)
                {
                    raw.Parse(ref momentsTuples);
                    Console.WriteLine(raw.Stdout);

                    foreach (Tuple<double, string, double> t in momentsTuples.Readings)
                        momentsArrays.Readings.Add(new List<string>() { t.Item1.ToString(), t.Item2.ToString(), t.Item3.ToString() });

                    switch (format)
                    {
                        case OutputFormat.Csv:
                            {
                                Console.WriteLine(momentsTuples.ToString());
                            }
                            break;

                        case OutputFormat.Json:
                            {
                                Console.WriteLine(JsonConvert.SerializeObject(momentsArrays, Formatting.Indented));
                            }
                            break;

                        default:
                            throw new InvalidOperationException();
                    }
                }

                if (validate)
                {
                    raw.debug = false;

                    if (raw.TryParse(ref momentsTuples))
                        Console.WriteLine("File has a valid format.");
                    else
                        Console.WriteLine("File has an invalid format.");
                }

                return Helpers.FondFarewell();
            }
            catch (Exception ex)
            {
                Bmon.Client.Core.Echo.Proxy.Caught.Msg(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return Helpers.AngryFarewell(ex);
            }
        }
    }
}
