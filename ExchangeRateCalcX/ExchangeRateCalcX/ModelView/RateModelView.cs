using ExchangeRateCalcX.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static ExchangeRateCalcX.API.Rootobject;

namespace ExchangeRateCalcX.Views
{
    public class RateModelView
    {
        //we get both values from 1 request, so save both, ex. MXN to CAN & CAN to MXN
        public string strRateFrom { get; set; }
        public string strRateTo { get; set; }
        public float curValue { get; set; }
        
        public string strRateFromSecondary { get; set; }
        public string strRateToSecondary { get; set; }
        public float curValueSecondary { get; set; }

        public RateModelView GetRateValues(Rootobject rate)
        {
            return this;
        }
        public void InsertRate(DatabaseManager dbConnection, Rootobject rate)
        {
            foreach (var currencyRate in rate.results.rateList)
            {
                var key = currencyRate.Key;
                var item = JsonConvert.DeserializeObject<Rate>(currencyRate.Value.ToString());


                tblExchangeRates newRate = new tblExchangeRates();
                newRate.ID = null;
                newRate.strRateFrom = item.fr;
                newRate.curRateFrom = item.val;
                newRate.strRateTo = item.to;
                newRate.curRateTo = 0;
                newRate.dtInserted = DateTime.Now.ToString();
                newRate.dtLastRevised = DateTime.Now.ToString();
                newRate.IsFavorite = 0;
                dbConnection.InsertRate(newRate);
                //TODO try catch instead of returning
            }
        }
    }
}
