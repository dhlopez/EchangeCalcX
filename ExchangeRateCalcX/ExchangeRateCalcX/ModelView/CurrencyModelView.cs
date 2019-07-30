using ExchangeRateCalcX.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static ExchangeRateCalcX.API.Currency;

namespace ExchangeRateCalcX.ModelView
{
    public class CurrencyModelView
    {
        public void InserCurrency(DatabaseManager dbConnection, Currency currency)
        {
            foreach (var currencyItem in currency.results.currencyList)
            {
                
                //var key = currencyRate.Key;
                var item = JsonConvert.DeserializeObject<CurrencyItem>(currencyItem.Value.ToString());
                tblCurrencies newCurrency = new tblCurrencies();
                newCurrency.strCurrencyID = "";
                newCurrency.strCurrencyName = "";
                dbConnection.InsertCurrency(newCurrency);
                //TODO try catch instead of returning
            }
        }
    }
}
