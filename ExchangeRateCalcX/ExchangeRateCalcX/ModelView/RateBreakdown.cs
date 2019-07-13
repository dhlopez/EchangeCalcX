using ExchangeRateCalcX.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static ExchangeRateCalcX.API.Rootobject;

namespace ExchangeRateCalcX.Views
{
    public class RateBreakdown
    {
        //we get both values from 1 request, so save both, ex. MXN to CAN & CAN to MXN
        public string strRateFrom { get; set; }
        public string strRateTo { get; set; }
        public float curValue { get; set; }
        
        public string strRateFromSecondary { get; set; }
        public string strRateToSecondary { get; set; }
        public float curValueSecondary { get; set; }

        public RateBreakdown GetRateValues(Rootobject rate)
        {
            //this.strRateFrom = rate.results.from_to.fr;
            //this.strRateTo = rate.results.from_to.to;
            //this.curValue = rate.results.from_to.val;

            //this.strRateFromSecondary = rate.results.to_from.fr;
            //this.strRateToSecondary = rate.results.to_from.to;
            //this.curValueSecondary = rate.results.to_from.val;

            return this;
        }
        public int InsertPrimary(DatabaseManager dbConnection, Rootobject rate)
        {
            tblExchangeRates primaryRate = new tblExchangeRates();
            foreach (var currencyRate in rate.results.currencyList)
            {
                var key = currencyRate.Key;
                var item = JsonConvert.DeserializeObject<Rate>(currencyRate.Value.ToString()); 
                 
            }
            //primaryRate.strRateFrom = rate.results.from_to.fr;
            //primaryRate.curRateFrom = rate.results.from_to.val;
            //primaryRate.strRateTo = rate.results.from_to.to;
            //primaryRate.curRateTo = 0;
            //primaryRate.dtInserted = DateTime.Now.ToString();
            //primaryRate.dtLastRevised = DateTime.Now.ToString();
            //primaryRate.IsFavorite = 0;
            return dbConnection.InsertRate(primaryRate);
        }
        public int InsertSecondary(DatabaseManager dbConnection, Rootobject rate)
        {
            tblExchangeRates secondaryRate = new tblExchangeRates();
            //secondaryRate.strRateFrom = rate.results.to_from.fr;
            //secondaryRate.curRateFrom = rate.results.to_from.val;
            //secondaryRate.strRateTo = rate.results.to_from.to;
            //secondaryRate.curRateTo = 0;
            //secondaryRate.dtInserted = DateTime.Now.ToString();
            //secondaryRate.dtLastRevised = DateTime.Now.ToString();
            //secondaryRate.IsFavorite = 0;
            return dbConnection.InsertRate(secondaryRate);
        }
    }
}
