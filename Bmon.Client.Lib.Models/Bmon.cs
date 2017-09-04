using System;
using System.Collections.Generic;

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
    }
}
