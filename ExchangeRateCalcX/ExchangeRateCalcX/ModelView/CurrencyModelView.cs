using ExchangeRateCalcX.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static ExchangeRateCalcX.Model.CurrencyToken;

namespace ExchangeRateCalcX.ModelView
{
    public class CurrencyModelView
    {
        
        public void PrepareCurrencyForInsert(Model.CurrencyToken.Rootobject currency)
        {
            foreach (var currencyItem in currency.results.currencyList)
            {
                //var key = currencyRate.Key;
                var item = JsonConvert.DeserializeObject<Currency>(currencyItem.Value.ToString());
                Currency newCurrency = new Currency();
                newCurrency.id = item.id;
                newCurrency.id = item.currencyName;
                DatabaseManager.InsertCurrency(newCurrency);
                //TODO try catch instead of returning
            }
        }

        public async void VerifyAndInsertCurrencies(APIService apiService)
        {
            bool exists = DatabaseManager.VerifyIfListOfCurrenciesExists();

            if (!exists)
            {
                CurrencyToken.Rootobject currency;
                //if no records, get them from the api
                currency = await apiService.GetListOfCurrenciesFromAPI();
                PrepareCurrencyForInsert(currency);
            }

            GetTblCurrencies();
        }

        public ObservableCollection<CurrencyToken.Rootobject> GetTblCurrencies()
        {
            return DatabaseManager.GetAllCurrencies();
        }
    }
}