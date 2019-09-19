using ExchangeRateCalcX.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void MapRatesForInsert(Rootobject rates)
        {
            foreach (var currencyRate in rates.results.rateList)
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
                DatabaseManager.InsertRate(newRate);
                //TODO try catch instead of returning
            }
        }
        public async Task HandleDifferentCurrencySelection(APIService apiService, tblCurrencies selectedFromCurrency, tblCurrencies selectedToCurrency)
        {
            Rootobject rate = new Rootobject();
            if (!(selectedFromCurrency == null) && !(selectedToCurrency == null))
            {
                rate = await apiService.GetRate(selectedFromCurrency.strCurrencyID, selectedToCurrency.strCurrencyID);
                MapRatesForInsert(rate);           
            }
            //return rate;
        }

        public tblExchangeRates GetCurrentRateFromDB(tblCurrencies selectedFromCurrency, tblCurrencies selectedToCurrency)
        {
            List<tblExchangeRates> retrievedRates = new List<tblExchangeRates>();
            tblExchangeRates rateToReturn = new tblExchangeRates();
            if (selectedFromCurrency != null && selectedToCurrency != null)
            {
                retrievedRates = DatabaseManager.ReadRate(selectedFromCurrency.strCurrencyID, selectedToCurrency.strCurrencyID);
            }
            
            if (retrievedRates.Count==1)
            {
                rateToReturn = retrievedRates.FirstOrDefault(); //(tblExchangeRates) retrievedRates.Where(r => r.strRateFrom.Equals(selectedFromCurrency.strCurrencyID)).FirstOrDefault();
            }
            return rateToReturn;
        }
    }
}
