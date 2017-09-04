using System;
using System.Collections.Generic;

namespace Bmon.Client.Lib.Models
{
    [Serializable]
    public class BmonTrendsForPost
    {
        public string StoreKey { get; set; }
        public List<Tuple<double, string, double>> Readings { get; set; }

        public BmonTrendsForPost()
        {
            StoreKey = string.Empty;
            Readings = new List<Tuple<double, string, double>>();
        }

        public BmonTrendsForPost(string storeKey)
        {
            StoreKey = storeKey;
            Readings = new List<Tuple<double, string, double>>();
        }

        public BmonTrendsForPost(string storeKey, List<Tuple<double, string, double>> readings)
        {
            StoreKey = storeKey;
            Readings = readings;
        }
    }
}
