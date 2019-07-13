using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExchangeRateCalcX
{
    public class tblExchangeRates
    {
        public int ID { get; set; }

        public string strRateFrom { get; set; }

        public double curRateFrom { get; set; }

        public string strRateTo { get; set; }

        public double curRateTo { get; set; }

        public string dtInserted { get; set; }

        public string dtLastRevised { get; set; }

        public int IsFavorite { get; set; }
    }
}