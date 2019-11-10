using ExchangeRateCalcX.Model;
using ExchangeRateCalcX.ViewModel;
using ExchangeRateCalcX.Views;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateCalcX.Model
{
    public class APIService
    {
        StringBuilder urlRate;
        StringBuilder urlCurList = new StringBuilder();
        StringBuilder urlSite = new StringBuilder();
        StringBuilder urlKey = new StringBuilder();
        StringBuilder urlCurrencyList = new StringBuilder();
        StringBuilder urlConvert = new StringBuilder();

        //private const string UrlRate = "https://free.currconv.com/api/v7/convert?q=CAD_MXN,MXN_CAD&apiKey=5f8325b1a91b04d0a655"; //This url is a free public api intended for demos
        //private const string UrlCurrencyList = "https://free.currconv.com/api/v7/currencies?apiKey=5f8325b1a91b04d0a655"; //This url is a free public api intended for demos
        private readonly HttpClient _client = new HttpClient(); //Creating a new instance of HttpClient. (Microsoft.Net.Http)
        public RateToken.Rootobject rateToken;
        public CurrencyToken.Rootobject currencyToken;
        CalcViewModel currentRateBreakdown;

        public APIService()
        {
            urlSite.Insert(0, "https://free.currconv.com/api/v7/");
            urlKey.Insert(0, "apiKey=5f8325b1a91b04d0a655");
            urlCurrencyList.Insert(0, "currencies?");
            urlConvert.Insert(0, "convert?q=");
        }

        public async Task<RateToken.Rootobject> GetRate(string fromCurrency, string toCurrency)
        {
            urlRate = new StringBuilder();
            urlRate.AppendFormat("{0}{1}{2}_{3},{3}_{2}&{4}", urlSite, urlConvert, fromCurrency, toCurrency, urlKey);
            string response =  await _client.GetStringAsync(urlRate.ToString());

            rateToken = JsonConvert.DeserializeObject<RateToken.Rootobject>(response);

            return rateToken;
        }

        public async Task<CurrencyToken.Rootobject> GetListOfCurrenciesFromAPI()
        {
            urlCurList.AppendFormat("{0}{1}{2}", urlSite, urlCurrencyList, urlKey);
            string response =  await _client.GetStringAsync(urlCurList.ToString());

            currencyToken = JsonConvert.DeserializeObject<CurrencyToken.Rootobject>(response);

            return currencyToken;
        }
    }
}
