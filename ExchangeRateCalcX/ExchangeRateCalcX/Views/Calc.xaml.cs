using ExchangeRateCalcX.Model;
using ExchangeRateCalcX.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static ExchangeRateCalcX.Model.CurrencyToken;
using static ExchangeRateCalcX.Model.RateToken;

namespace ExchangeRateCalcX.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calc : ContentPage
    {
        public Calc()
        {
            InitializeComponent();
        }

        protected override /*async*/ void OnAppearing()
        {
            
            //apiService = new APIService();
            //rateModelView = new RateModelView();
            //currencyModelView = new CurrencyModelView();
            //currencyModelView.VerifyAndInsertCurrencies(apiService);
            //listOfCurrencies = currencyModelView.GetTblCurrencies(); 
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            //FromAmtValue = button.Text;
        }

        private void Clear_Clicked(object sender, EventArgs e)
        {
            //FromAmtValue = "";
        }

        private void Insert_Clicked(object sender, EventArgs e)
        {
            //rateModelView.GetRateMV(apiService);
        }

        private async void Select_Clicked(object sender, EventArgs e)
        {
            List<Rate> rates = await DatabaseManager.GetAllRates();
        }

        //private async void CurrencyPick_SelectedIndexChanged(object sender, EventArgs e)
        //{
            //selectedFromCurrency = FromCurrencyPick.SelectedItem as Currency;
            //selectedToCurrency = ToCurrencyPick.SelectedItem as Currency;
            
            //await rateModelView.HandleDifferentCurrencySelection(apiService, selectedFromCurrency, selectedToCurrency);
            
        //}

        private void Button_Clicked_1(object sender, EventArgs e)
        {            
            DisplayAlert("Alert", "Insert variable for testing here" + this.Id/*rateModelView.CurrentSelectedRate*/, "OK");            
        }
    }
}