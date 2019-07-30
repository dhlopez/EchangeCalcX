using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateCalcX.API
{
    public class Rootobject
    {
        //public class Rootobject
        //{
        //    public Query query { get; set; }
        //    public Results results { get; set; }
        //}


        public Query query { get; set; }
        public Results results { get; set; }

        public class Query
        {
            public int count { get; set; }
        }

        public class Results
        {
            [JsonExtensionData]
            public IDictionary<string, JToken> rateList;// = new Dictionary<string, Rate>();

            //public FROM_TO from_to { get; set; }
            //public TO_FROM to_from { get; set; }
        }

        public class Rate
        {
            public string id { get; set; }
            public string fr { get; set; }
            public string to { get; set; }
            public float val { get; set; }
        }

        //public class TO_FROM
        //{
        //    public string id { get; set; }
        //    public string fr { get; set; }
        //    public string to { get; set; }
        //    public float val { get; set; }
        //}

    }
}
