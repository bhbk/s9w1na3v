using Bmon.Client.Lib.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bmon.Client.Lib.Devour.DotCsv
{
    public class GenericFormatA
    {
        private FileStream fs;
        private StreamReader sr;
        private CsvParser csvr;
        private CsvConfiguration conf;

        public GenericFormatA(string file)
        {
            fs = File.OpenRead(file);
            conf = new CsvConfiguration();
        }
        public BmonPostTrendMultiple ParseTrends()
        {
            BmonPostTrendMultiple trend;
            List<Tuple<string, string, string>> trendMeta;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            DateTime timeStamp;
            string[] row;

            try
            {
                using (sr = new StreamReader(fs))
                using (csvr = new CsvParser(sr, conf))
                {
                    /*
                     * skipping first line that has bad format.
                     */
                    row = csvr.Read();
                    trendMeta = new List<Tuple<string, string, string>>();

                    while (true)
                    {
                        row = csvr.Read();

                        /*
                         * exit this loop when we his first empty row. at this point we should know how
                         * trend points exist for processing in the next loop.
                         */
                        if (row[0] == string.Empty)
                            break;

                        /*
                         * generate a 3-tuple to store meta data about trend points.
                         */
                        if (row[0].ToLower().Contains("point"))
                            trendMeta.Add(new Tuple<string, string, string>(row[0].TrimEnd(':'), row[1], row[2]));

                        if (Statics.ConfDebug)
                        {
                            switch (row.Length)
                            {
                                case 2:
                                    Console.WriteLine(string.Format("{0} {1}", row[0], row[1]));
                                    break;

                                case 4:
                                    Console.WriteLine(string.Format("{0} {1} {2} {3}", row[0].TrimEnd(':'), row[1], row[2], row[3]));
                                    break;

                                default:
                                    throw new CsvBadDataException();
                            }
                        }
                    }

                    /*
                     * skipping the header line that has all the trend points after it.
                     */
                    row = csvr.Read();

                    if (Statics.ConfDebug)
                        Console.WriteLine();

                    while (true)
                    {
                        timeStamp = new DateTime();
                        trend = new BmonPostTrendMultiple();
                        row = csvr.Read();

                        /*
                         * ensure the number of fields in these rows match the number of trend points calculated in 
                         * the previous loop. do not forget to add the date & time field to the total count.
                         */
                        if (row.Length == (trendMeta.Count + 2))
                        {
                            /*
                             * ensure the date & time fields can be properly parsed after they are concatenated. throw
                             * exception if not.
                             */
                            if (!DateTime.TryParse(row[0] + " " + row[1], out timeStamp))
                                throw new CsvBadDataException();
                                                       
                            for (int i = 0; i < trendMeta.Count; i++)
                            {
                                Tuple<double, string, double> moment;
                                double seconds, reading;

                                /*
                                 * ensure the reading field can be properly parsed. skip if not.
                                 */
                                if (double.TryParse(row[i + 2], out reading))
                                {
                                    seconds = (timeStamp.ToUniversalTime() - epoch).TotalSeconds;
                                    moment = new Tuple<double, string, double>(seconds, trendMeta.ElementAt(i).Item2, double.Parse(row[i + 2]));

                                    trend.Readings.Add(moment);

                                    if (Statics.ConfDebug)
                                        Console.WriteLine(string.Format("{0} {1} {2}", moment.Item1, moment.Item2, moment.Item3));
                                }
                            }

                            if (Statics.ConfDebug)
                                Console.WriteLine();
                        }
                        else if (row == null)
                            break;
                        else
                            break;
                    }

                    return trend;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }

            return null;
        }
    }
}
