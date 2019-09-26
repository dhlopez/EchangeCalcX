using ExchangeRateCalcX.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExchangeRateCalcX.Model.RateToken;

namespace ExchangeRateCalcX.Views
{
    public class RateModelView
    {
        public APIService apiService;
        public RateModelView GetRateValues(Rate rate)
        {
            return this;
        }
        
        public void MapRatesForInsert(RateToken.Rootobject rates)
        {
            foreach (var currencyRate in rates.results.rateList)
            {
                
                var item = JsonConvert.DeserializeObject<Rate>(currencyRate.Value.ToString());

                Rate newRate = MapApiTokenToRate(item);
                DatabaseManager.InsertRate(newRate);
            }
        }

        public Rate MapApiTokenToRate(Rate rate)
        {
            Rate newRate = new Rate();
            newRate.id = null;
            newRate.from = rate.from;
            newRate.val = rate.val;
            newRate.to = rate.to;
            newRate.dtInserted = DateTime.Now.ToString();
            newRate.IsFavorite = 0;
            return newRate;
        }

        public async Task<Rate> HandleDifferentCurrencySelection(APIService apiService, CurrencyToken.Currency selectedFromCurrency, CurrencyToken.Currency selectedToCurrency)
        {
            RateToken.Rootobject rateApi = new RateToken.Rootobject();
            Rate rate = new Rate();
            List<Rate> retrievedRates = new List<Rate>();
            Rate rateToReturn = new Rate();

            if (!(selectedFromCurrency == null) && !(selectedToCurrency == null))
            {
                retrievedRates = DatabaseManager.ReadRate(selectedFromCurrency.id, selectedToCurrency.id);

                if (retrievedRates.Count == 1)
                {
                    rateToReturn = retrievedRates.FirstOrDefault();
                    if (DateTime.Parse(rateToReturn.dtInserted) >= DateTime.Now.AddHours(-6))
                    {
                        return rateToReturn;
                    }
                }
                else
                {
                    rateApi = await apiService.GetRate(selectedFromCurrency.id, selectedToCurrency.id);

                    if (rateApi != null)
                    {
                        MapRatesForInsert(rateApi);
                    }

                    rate = JsonConvert.DeserializeObject<RateToken.Rate>(rateApi.results.rateList.Where(r => r.Key == selectedFromCurrency.id + "_" + selectedToCurrency.id).ToString());

                    rateToReturn = MapApiTokenToRate(rate);

                    return rateToReturn;
                }
                
            }
            return rateToReturn;
        }
    }
}
