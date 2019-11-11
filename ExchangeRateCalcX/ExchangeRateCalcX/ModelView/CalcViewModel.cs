using ExchangeRateCalcX.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static ExchangeRateCalcX.Model.CurrencyToken;
using static ExchangeRateCalcX.Model.RateToken;

namespace ExchangeRateCalcX.ViewModel
{
    public class CalcViewModel : INotifyPropertyChanged
    {
        double currentSelectedRate;
        APIService apiService;

        public ObservableCollection<Currency> FromCurrencies { get; set; }
        public ObservableCollection<Currency> ToCurrencies { get; set; }

        private Currency selectedFromCurrency;
        private Currency selectedToCurrency;
               
        public Currency SelectedFromCurrency
        {
            get
            {
                return selectedFromCurrency;
            }
            set
            {
                if (value != null)
                {
                    if (value.id != null)
                    {
                        selectedFromCurrency = value;
                        NotifyPropertyChanged();
                        HandleDifferentCurrencySelection();
                    }
                }
            }
        }

        public Currency SelectedToCurrency
        {
            get
            {
                return selectedToCurrency;
            }
            set
            {
                if(value != null)
                {
                    if (value.id != null)
                    {
                        selectedToCurrency = value;
                        NotifyPropertyChanged();
                        HandleDifferentCurrencySelection();
                    }
                }
            }
        }

        private double curFrom;

        private double curTo;
        public double CurFrom
        {
            get { return curFrom; }
            set
            {
               curFrom = value;
               NotifyPropertyChanged();               
            }
        }

        public double CurTo
        {
            get { return curTo; }
            set
            {
                curTo = value;
                NotifyPropertyChanged();
            }
        }

        //public APIService apiService;

        public CalcViewModel()
        {
            DatabaseManager.DeleteAllCurrencies();
            CurFrom = 0;
            CurTo = 0;
            SelectedFromCurrency = new Currency();
            SelectedToCurrency = new Currency();
            FromCurrencies = new ObservableCollection<Currency>();
            ToCurrencies = new ObservableCollection<Currency>();
            apiService = new APIService();

            ButtonNumberClickHandler = new Command<string>((value) =>
            {
                var a = value;
            });


            Start();   
        }

        public async void  Start()
        {
            await VerifyAndInsertCurrencies(apiService);

            foreach (var item in await GetCurrenciesFromDB())
            {
                FromCurrencies.Add(item);
            }

            foreach (var item in await GetCurrenciesFromDB())
            {
                ToCurrencies.Add(item);
            }
            
        }

        public double CurrentSelectedRate
        {
            get { return currentSelectedRate; }
            set
            {
                currentSelectedRate = value;
                NotifyPropertyChanged();
                //HandleDifferentCurrencySelection();
            }
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
            newRate.id = rate.id;
            newRate.fr = rate.fr;
            newRate.val = rate.val;
            newRate.to = rate.to;
            newRate.dtInserted = DateTime.Now.ToString();
            newRate.IsFavorite = 0;
            return newRate;
        }

        public async void HandleDifferentCurrencySelection() //used to be a task
        {
            RateToken.Rootobject rateApi = new RateToken.Rootobject();
            Rate rate = new Rate();
            List<Rate> retrievedRates = new List<Rate>();

            if ((SelectedFromCurrency != null) && (SelectedToCurrency != null))
            {
                if ((SelectedFromCurrency.id != null) && (SelectedToCurrency.id != null))
                {
                    retrievedRates = await DatabaseManager.ReadRate(SelectedFromCurrency.id, SelectedToCurrency.id);

                    if (retrievedRates.Count == 1)
                    {
                        if (DateTime.Parse(retrievedRates.FirstOrDefault().dtInserted) >= DateTime.Now.AddHours(-6))
                        {
                            rate = retrievedRates.FirstOrDefault();
                        }
                    }

                    if (retrievedRates.Count == 0)
                    {
                        rateApi = await apiService.GetRate(SelectedFromCurrency.id, SelectedToCurrency.id);

                        if (rateApi != null)
                        {
                            if(rateApi.results != null)
                                MapRatesForInsert(rateApi);
                        }

                        try
                        {
                            var a = rateApi.results.rateList.Where(r => r.Key == (SelectedFromCurrency.id.ToString() + "_" + SelectedToCurrency.id.ToString())).ToList();
                            var b = a.FirstOrDefault().Value.ToString();
                            rate = JsonConvert.DeserializeObject<Rate>(b);

                            rate = MapApiTokenToRate(rate);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }

            double curFrom = rate.val;
            CurrentSelectedRate = curFrom;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task MapApiCurrencyForDBInsert(CurrencyToken.Rootobject currency)
        {
            foreach (var currencyItem in currency.results.currencyList)
            {
                var item = JsonConvert.DeserializeObject<Currency>(currencyItem.Value.ToString());
                Currency newCurrency = new Currency();
                newCurrency.id = item.id;
                newCurrency.currencyName = item.currencyName;
                newCurrency.currencySymbol = item.currencySymbol;
                await DatabaseManager.InsertCurrency(newCurrency);
            }
        }

        public async Task VerifyAndInsertCurrencies(APIService apiService)
        {
            bool exists = await DatabaseManager.VerifyIfListOfCurrenciesExists();

            if (!exists)
            {
                CurrencyToken.Rootobject currencies;
                //if no records, get them from the api
                currencies = await apiService.GetListOfCurrenciesFromAPI();
                await MapApiCurrencyForDBInsert(currencies);
            }

            await GetCurrenciesFromDB();
        }

        public async Task<ObservableCollection<Currency>> GetCurrenciesFromDB()
        {
            return await DatabaseManager.GetAllCurrencies();
        }

        public ICommand ButtonNumberClickHandler { private set; get; }
    }
}
