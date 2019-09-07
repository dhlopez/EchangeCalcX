using ExchangeRateCalcX.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static ExchangeRateCalcX.API.Rootobject;

namespace ExchangeRateCalcX.API
{
    public class APIService
    {
        private const string UrlRate = "https://free.currconv.com/api/v7/convert?q=CAD_MXN,MXN_CAD&apiKey=5f8325b1a91b04d0a655"; //This url is a free public api intended for demos
        private const string UrlCurrencyList = "https://free.currconv.com/api/v7/currencies?apiKey=5f8325b1a91b04d0a655"; //This url is a free public api intended for demos
        private readonly HttpClient _client = new HttpClient(); //Creating a new instance of HttpClient. (Microsoft.Net.Http)
        public Rootobject rate;
        RateModelView currentRateBreakdown;

        public async Task<Rootobject> GetRate()
        {
            string content =  await _client.GetStringAsync(UrlRate);

            var rate = JsonConvert.DeserializeObject<Rootobject>(content);

            return rate;
        }

        public async Task<Currency.Rootobject> GetListOfCurrenciesFromAPI()
        {
            string content =  await _client.GetStringAsync(UrlCurrencyList);

            var currency = JsonConvert.DeserializeObject<Currency.Rootobject>(content);

            return currency;
        }
    }
}
