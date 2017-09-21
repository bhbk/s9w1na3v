using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bmon.Client.Lib.Models
{
    public abstract class MomentModel
    {
        internal string StoreKey { get; set; }

        internal MomentModel() { }

        internal MomentModel(string storekey)
        {
            StoreKey = storekey;
        }
    }

    [Serializable]
    public class MomentArrays : MomentModel
    {
        public List<List<string>> Readings { get; set; }

        public MomentArrays()
            : base()
        {
            StoreKey = string.Empty;
            Readings = new List<List<string>>();
        }

        public MomentArrays(string key)
            : base(key)
        {
            StoreKey = key;
            Readings = new List<List<string>>();
        }

        public override string ToString()
        {
            StringBuilder moments = new StringBuilder();

            foreach (List<string> moment in Readings)
            {
                moments.Append(string.Format("{0}, {1}, {2}", moment[0], moment[1], moment[2]));
                moments.Append(Environment.NewLine);
            }

            return moments.ToString();
        }
    }

    [Serializable]
    public class MomentTuples : MomentModel
    {
        public List<Tuple<double, string, double>> Readings { get; set; }

        public MomentTuples()
            : base()
        {
            StoreKey = string.Empty;
            Readings = new List<Tuple<double, string, double>>();
        }

        public MomentTuples(string key)
            : base(key)
        {
            StoreKey = key;
            Readings = new List<Tuple<double, string, double>>();
        }

        public override string ToString()
        {
            StringBuilder moments = new StringBuilder();

            foreach (Tuple<double, string, double> moment in Readings.OrderBy(x => x.Item2))
            {
                moments.Append(string.Format("{0}, {1}, {2}", moment.Item1.ToString(), moment.Item2.ToString(), moment.Item3.ToString()));
                moments.Append(Environment.NewLine);
            }

            return moments.ToString();
        }
    }
}
