using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateCalcX.API
{
    public class Currency
    {
        public Results results { get; set; }
        public class Rootobject
        {
            public Results results { get; set; }
        }

        public class Results
        {
            [JsonExtensionData]
            public IDictionary<string, JToken> currencyList;
        }

        public class CurrencyItem
        {
            public string currencyName { get; set; }
            public string currencySymbol { get; set; }
            public string id { get; set; }
        }

    }
}
