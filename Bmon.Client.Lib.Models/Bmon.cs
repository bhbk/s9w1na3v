﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bmon.Client.Lib.Models
{
    [Serializable]
    public class BmonPostTrendMultiple
    {
        public string StoreKey { get; set; }
        public List<Tuple<double, string, double>> Readings { get; set; }

        public BmonPostTrendMultiple()
        {
            StoreKey = string.Empty;
            Readings = new List<Tuple<double, string, double>>();
        }

        public BmonPostTrendMultiple(string storeKey)
        {
            StoreKey = storeKey;
            Readings = new List<Tuple<double, string, double>>();
        }

        public BmonPostTrendMultiple(string storeKey, List<Tuple<double, string, double>> readings)
        {
            StoreKey = storeKey;
            Readings = readings;
        }

        public BmonPostTrendMultiple(List<Tuple<double, string, double>> readings)
        {
            StoreKey = string.Empty;
            Readings = readings;
        }

        public override string ToString()
        {
            StringBuilder trends = new StringBuilder();

            foreach (Tuple<double, string, double> moment in Readings.OrderBy(x => x.Item2))
            {
                trends.Append(string.Format("{0} {1} {2}", moment.Item1.ToString(), moment.Item2, moment.Item3.ToString()));
                trends.Append(Environment.NewLine);
            }

            return trends.ToString();
        }
    }
}
