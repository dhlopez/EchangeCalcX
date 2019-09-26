using ExchangeRateCalcX.Model;
using ExchangeRateCalcX.Model;
using ExchangeRateCalcX.ModelView;
using ExchangeRateCalcX.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static ExchangeRateCalcX.Model.CurrencyToken;
using static ExchangeRateCalcX.Model.RateToken;

namespace ExchangeRateCalcX.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Calc : ContentPage, INotifyPropertyChanged  
	{
        decimal toAmt = 0;
        string fromAmtValue = "";
        double currentSelectedRate = 0;

        APIService apiService;
        //Rootobject currentRate;
        RateModelView rateModelView;
        CurrencyModelView currencyModelView;
        Rate selectedTblExchangeRates;

        public ObservableCollection<CurrencyToken.Rootobject> fromCurrencies { get; set; }
        public ObservableCollection<CurrencyToken.Rootobject> toCurrencies { get; set; }

        public Currency selectedFromCurrency { get; set; }
        public Currency selectedToCurrency { get; set; }

        public List<Currency> listOfCurrencies;

        public string FromAmtValue
        {
            get { return fromAmtValue; }
            set {
                fromAmtValue += value;
                NotifyPropertyChanged();
            }
        }

        public double CurrentSelectedRate
        {
            get { return currentSelectedRate; }
            set {
                currentSelectedRate = value;
                NotifyPropertyChanged();
            }
        }

        public Calc ()
		{
			InitializeComponent ();
            apiService = new APIService();
            rateModelView = new RateModelView();
            currencyModelView = new CurrencyModelView();
            currencyModelView.VerifyAndInsertCurrencies(apiService);

            fromCurrencies = currencyModelView.GetTblCurrencies();
            toCurrencies = currencyModelView.GetTblCurrencies();

            FromCurrencyPick.ItemsSource = fromCurrencies;
            ToCurrencyPick.ItemsSource = toCurrencies;
		}

        protected override void OnAppearing()
        {
            //DatabaseManager.DeleteAllCurrencies();
            //apiService = new APIService();
            //rateModelView = new RateModelView();
            //currencyModelView = new CurrencyModelView();
            //currencyModelView.VerifyAndInsertCurrencies(apiService);
            //listOfCurrencies = currencyModelView.GetTblCurrencies(); 
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            FromAmtValue = button.Text;
        }

        public event PropertyChangedEventHandler PropertyChanged;  

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")  
        {  
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Clear_Clicked(object sender, EventArgs e)
        {
            FromAmtValue = "";
        }

        private void Insert_Clicked(object sender, EventArgs e)
        {
            //rateModelView.GetRateMV(apiService);
        }

        private void Select_Clicked(object sender, EventArgs e)
        {
            List<Rate> rates = DatabaseManager.GetAllRates();
        }

        private async void CurrencyPick_SelectedIndexChanged(object sender, EventArgs e)
        {
            double curFrom = 0;
            selectedFromCurrency = FromCurrencyPick.SelectedItem as Currency;
            selectedToCurrency = ToCurrencyPick.SelectedItem as Currency;
            selectedTblExchangeRates = await rateModelView.HandleDifferentCurrencySelection(apiService, selectedFromCurrency, selectedToCurrency);
            Double.TryParse(selectedTblExchangeRates.from, out curFrom);

            CurrentSelectedRate = curFrom;


        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (CurrentSelectedRate != 0)
            {
                DisplayAlert("Alert", currentSelectedRate.ToString(), "OK");
            }
        }
    }
}