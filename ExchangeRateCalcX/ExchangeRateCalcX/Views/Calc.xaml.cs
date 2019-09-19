using ExchangeRateCalcX.API;
using ExchangeRateCalcX.ModelView;
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

        public ObservableCollection<tblCurrencies> fromCurrencies { get; set; }
        public ObservableCollection<tblCurrencies> toCurrencies { get; set; }

        public tblCurrencies selectedFromCurrency { get; set; }
        public tblCurrencies selectedToCurrency { get; set; }

        public List<tblCurrencies> listOfCurrencies;

        public string FromCur { get; set; }
        public string ToCur { get; set; }
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
            int value = 0;

            FromAmtValue = button.Text;

            //int.TryParse(button.Text, out value);
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
            List<tblExchangeRates> rates = DatabaseManager.GetAllRates();
        }

        private async void FromCurrencyPick_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedFromCurrency = FromCurrencyPick.SelectedItem as tblCurrencies;
            await rateModelView.HandleDifferentCurrencySelection(apiService, selectedFromCurrency, selectedToCurrency);

            //if(rateModelView.GetCurrentRateFromDB(selectedFromCurrency, selectedToCurrency).ID > 0)
            //{
                CurrentSelectedRate = rateModelView.GetCurrentRateFromDB(selectedFromCurrency, selectedToCurrency).curRateFrom;
            //}
        }

        private async void ToCurrencyPick_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedToCurrency = ToCurrencyPick.SelectedItem as tblCurrencies;
            await rateModelView.HandleDifferentCurrencySelection(apiService, selectedFromCurrency, selectedToCurrency);
            //if(rateModelView.GetCurrentRateFromDB(selectedFromCurrency, selectedToCurrency).ID > 0)
            //{
                CurrentSelectedRate = rateModelView.GetCurrentRateFromDB(selectedFromCurrency, selectedToCurrency).curRateFrom;
            //}
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