using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExchangeRateCalcX.Model
{
    public class CurrencyToken
    {
        public class Rootobject
        {
            public Results results { get; set; }
        }

        public class Results
        {
            [JsonExtensionData]
            public IDictionary<string, JToken> currencyList;
        }

        public class Currency
        {
            public string currencyName { get; set; }
            public string currencySymbol { get; set; }
            public string id { get; set; }
        }

    }
}
