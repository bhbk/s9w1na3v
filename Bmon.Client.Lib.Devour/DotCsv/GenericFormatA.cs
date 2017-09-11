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
        private string InputFile;
        private FileStream Fs;
        private StreamReader Sr;
        private CsvParser Csvr;
        private CsvConfiguration CsvConf;
        private DateTime Epoch;
        private DateTime TimeStamp;
        private string[] Row;
        public bool Debug { get; set; }
        private StringBuilder StandardOut;

        public StringBuilder Output
        {
            get
            {
                return StandardOut;
            }
        }

        public GenericFormatA(string file)
        {
            InputFile = file;
            CsvConf = new CsvConfiguration();
            Epoch = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeStamp = new DateTime();
            Debug = false;
            StandardOut = new StringBuilder();
        }

        public void Parse(ref MomentTuples trends)
        {
            Process(ref trends, true);
        }

        public bool TryParse(ref MomentTuples trends)
        {
            try
            {
                Process(ref trends, false);
                return true;
            }
            catch (CsvBadDataException ex)
            {
                StandardOut.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
                return false;
            }
        }

        private void Process(ref MomentTuples trends, bool parse)
        {
            using (Fs = File.OpenRead(InputFile))
            using (Sr = new StreamReader(Fs))
            using (Csvr = new CsvParser(Sr, CsvConf))
            {
                List<Tuple<string, string, string>> meta = new List<Tuple<string, string, string>>();

                /*
                 * skipping first line that has bad format.
                 */
                Row = Csvr.Read();

                while (true)
                {
                    Row = Csvr.Read();

                    /*
                     * exit this loop when we hit first empty row. at this point we should know how
                     * many trend moments exist for processing in the next loop.
                     */
                    if (Row[0] == string.Empty)
                        break;

                    /*
                     * generate a 3-tuple to store meta data about trend moments.
                     */
                    if (Row[0].ToLower().Contains("point"))
                        meta.Add(new Tuple<string, string, string>(Row[0].TrimEnd(':'), Row[1], Row[2]));

                    switch (Row.Length)
                    {
                        case 2:
                            if (Debug)
                                StandardOut.Append(string.Format("{0} {1}", Row[0], Row[1]));
                            break;

                        case 4:
                            if (Debug)
                                StandardOut.Append(string.Format("{0} {1} {2} {3}", Row[0].TrimEnd(':'), Row[1], Row[2], Row[3]));
                            break;

                        default:
                            throw new CsvBadDataException();
                    }
                }

                /*
                 * skipping the header line that has all the trend moments after it.
                 */
                Row = Csvr.Read();

                while (true)
                {
                    Row = Csvr.Read();

                    if (Row == null)
                        break;

                    /*
                     * ensure the number of fields in these rows match the number of trend moments calculated in 
                     * the previous loop. do not forget to add the date & time field to the total count.
                     */
                    else if (Row.Length == (meta.Count + 2))
                    {
                        /*
                         * ensure the date & time fields can be properly parsed after they are concatenated. throw
                         * exception if not.
                         */
                        if (!DateTime.TryParse(Row[0] + " " + Row[1], out TimeStamp))
                            throw new CsvBadDataException();

                        for (int i = 0; i < meta.Count; i++)
                        {
                            Tuple<double, string, double> moment;
                            double seconds, reading;

                            /*
                             * ensure the reading field can be properly parsed. skip if not.
                             */
                            if (double.TryParse(Row[i + 2], out reading))
                            {
                                seconds = (TimeStamp.ToUniversalTime() - Epoch).TotalSeconds;
                                moment = new Tuple<double, string, double>(seconds, meta.ElementAt(i).Item2, reading);

                                if (parse)
                                    trends.Readings.Add(moment);

                                if (Debug)
                                    StandardOut.Append(string.Format("{0} {1} {2}", moment.Item1, moment.Item2, moment.Item3));
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
