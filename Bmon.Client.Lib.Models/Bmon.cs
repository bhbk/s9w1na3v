using System;
using System.Collections.Generic;

namespace Bmon.Client.Lib.Models
{
    public class Bmon
    {
        [Serializable]
        public class HttpPost
        {
            public string storeKey { get; set; }
            public List<List<object>> readings { get; set; }
        }
    }
}
