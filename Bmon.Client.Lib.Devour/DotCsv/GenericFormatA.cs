using Bmon.Client.Lib.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bmon.Client.Lib.Devour.DotCsv
{
    public class GenericFormatA
    {
        private FileStream fs;
        private StreamReader sr;
        private CsvParser csvr;
        private CsvConfiguration conf;

        public GenericFormatA(string filePathAndName, bool throwOnBadData)
        {
            fs = File.OpenRead(filePathAndName);
            conf = new CsvConfiguration();
            conf.ThrowOnBadData = throwOnBadData;
        }
        public BmonTrendsForPost ParseTrends()
        {
            BmonTrendsForPost trends;
            List<Tuple<string, string, string>> meta;
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
                    meta = new List<Tuple<string, string, string>>();

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
                            meta.Add(new Tuple<string, string, string>(row[0].TrimEnd(':'), row[1], row[2]));

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
                        DateTime dt = new DateTime();
                        trends = new BmonTrendsForPost();
                        row = csvr.Read();

                        /*
                         * ensure the number of fields in these rows match the number of trend points calculated in 
                         * the previous loop. do not forget to add the date & time field to the total count.
                         */
                        if (row.Length == (meta.Count + 2))
                        {
                            /*
                             * ensure the date & time fields can be properly parsed after they are concatenated.
                             */
                            if (!DateTime.TryParse(row[0] + " " + row[1], out dt))
                                throw new CsvBadDataException();

                            for (int i = 0; i < meta.Count; i++)
                            {
                                Tuple<double, string, double> trend;
                                double utc, reading;

                                if (double.TryParse(row[i + 2], out reading))
                                {
                                    utc = DateTime.UtcNow.Subtract(dt).TotalSeconds;
                                    trend = new Tuple<double, string, double>(utc, meta.ElementAt(i).Item2, double.Parse(row[i + 2]));

                                    trends.Readings.Add(trend);

                                    if (Statics.ConfDebug)
                                        Console.WriteLine(string.Format("{0} {1} {2}", trend.Item1, trend.Item2, trend.Item3));
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

                    return trends;
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
