using Bmon.Client.Lib.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bmon.Client.Lib.Devour.DotCsv
{
    public class GenericFormatA
    {
        private string csv;
        private FileStream fs;
        private StreamReader sr;
        private CsvParser csvr;
        private CsvConfiguration conf;
        private DateTime epoch;
        private DateTime timeStamp;
        private string[] row;
        public bool debug { get; set; }
        private StringBuilder output;

        public StringBuilder Stdout
        {
            get
            {
                return output;
            }
        }

        public GenericFormatA(string file)
        {
            csv = file;
            conf = new CsvConfiguration();
            epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            timeStamp = new DateTime();
            debug = false;
            output = new StringBuilder();
        }

        public void Parse(ref MultipleMomentsTuples trends)
        {
            Process(ref trends, true);
        }

        public bool TryParse(ref MultipleMomentsTuples trends)
        {
            try
            {
                Process(ref trends, false);
                return true;
            }
            catch (CsvBadDataException ex)
            {
                output.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
                return false;
            }
        }

        private void Process(ref MultipleMomentsTuples trends, bool parse)
        {
            using (fs = File.OpenRead(csv))
            using (sr = new StreamReader(fs))
            using (csvr = new CsvParser(sr, conf))
            {
                List<Tuple<string, string, string>> meta = new List<Tuple<string, string, string>>();

                /*
                 * skipping first line that has bad format.
                 */
                row = csvr.Read();

                while (true)
                {
                    row = csvr.Read();

                    /*
                     * exit this loop when we hit first empty row. at this point we should know how
                     * many trend moments exist for processing in the next loop.
                     */
                    if (row[0] == string.Empty)
                        break;

                    /*
                     * generate a 3-tuple to store meta data about trend moments.
                     */
                    if (row[0].ToLower().Contains("point"))
                        meta.Add(new Tuple<string, string, string>(row[0].TrimEnd(':'), row[1], row[2]));

                    switch (row.Length)
                    {
                        case 2:
                            if (debug)
                                output.Append(string.Format("{0} {1}", row[0], row[1]));
                            break;

                        case 4:
                            if (debug)
                                output.Append(string.Format("{0} {1} {2} {3}", row[0].TrimEnd(':'), row[1], row[2], row[3]));
                            break;

                        default:
                            throw new CsvBadDataException();
                    }
                }

                /*
                 * skipping the header line that has all the trend moments after it.
                 */
                row = csvr.Read();

                while (true)
                {
                    row = csvr.Read();

                    if (row == null)
                        break;

                    /*
                     * ensure the number of fields in these rows match the number of trend moments calculated in 
                     * the previous loop. do not forget to add the date & time field to the total count.
                     */
                    else if (row.Length == (meta.Count + 2))
                    {
                        /*
                         * ensure the date & time fields can be properly parsed after they are concatenated. throw
                         * exception if not.
                         */
                        if (!DateTime.TryParse(row[0] + " " + row[1], out timeStamp))
                            throw new CsvBadDataException();

                        for (int i = 0; i < meta.Count; i++)
                        {
                            Tuple<double, string, double> moment;
                            double seconds, reading;

                            /*
                             * ensure the reading field can be properly parsed. skip if not.
                             */
                            if (double.TryParse(row[i + 2], out reading))
                            {
                                seconds = (timeStamp.ToUniversalTime() - epoch).TotalSeconds;
                                moment = new Tuple<double, string, double>(seconds, meta.ElementAt(i).Item2, reading);

                                if (parse)
                                    trends.Readings.Add(moment);

                                if (debug)
                                    output.Append(string.Format("{0} {1} {2}", moment.Item1, moment.Item2, moment.Item3));
                            }
                        }
                    }
                    else
                        break;
                }
            }
        }
    }
}
