using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateCalcX.Model
{
    public class RateToken
    {
        public class Rootobject
        {
            public Query query { get; set; }
            public Results results { get; set; }
        }

        public class Query
        {
            public int count { get; set; }
        }

        public class Results
        {
            [JsonExtensionData]
            public IDictionary<string, JToken> rateList;
        }

        public class Rate
        {
            public string id { get; set; }

            public double val { get; set; }
            public string to { get; set; }

            public string fr { get; set; }

            public string dtInserted { get; set; }
                
            public int IsFavorite { get; set; }
        }
   
    }
}
