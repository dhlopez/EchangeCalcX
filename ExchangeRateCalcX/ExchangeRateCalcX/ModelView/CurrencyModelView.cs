using ExchangeRateCalcX.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using static ExchangeRateCalcX.API.Currency;

namespace ExchangeRateCalcX.ModelView
{
    public class CurrencyModelView
    {
        
        public void PrepareCurrencyForInsert(Currency.Rootobject currency)
        {
            foreach (var currencyItem in currency.results.currencyList)
            {
                //var key = currencyRate.Key;
                var item = JsonConvert.DeserializeObject<CurrencyItem>(currencyItem.Value.ToString());
                tblCurrencies newCurrency = new tblCurrencies();
                newCurrency.strCurrencyID = item.id;
                newCurrency.strCurrencyDesc = item.currencyName;
                DatabaseManager.InsertCurrency(newCurrency);
                //TODO try catch instead of returning
            }
        }

        public async void VerifyAndInsertCurrencies(APIService apiService)
        {
            bool exists = DatabaseManager.VerifyIfListOfCurrenciesExists();

            if (!exists)
            {
                Currency.Rootobject currency;
                //if no records, get them from the api
                currency = await apiService.GetListOfCurrenciesFromAPI();
                PrepareCurrencyForInsert(currency);
            }

            GetTblCurrencies();
        }

        public ObservableCollection<tblCurrencies> GetTblCurrencies()
        {
            return DatabaseManager.GetAllCurrencies();
        }
    }
}