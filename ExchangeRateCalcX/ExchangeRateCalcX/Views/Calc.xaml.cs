using ExchangeRateCalcX.API;
using System;
using System.Collections.Generic;
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
        APIService apiService;
        Rootobject rate;
        DatabaseManager db;
        RateBreakdown rateBreakdown;

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

        public Calc ()
		{
			InitializeComponent ();
            BindingContext = this;
		}

        protected override async void OnAppearing()
        {
            apiService = new APIService();
            db = new DatabaseManager();
            rateBreakdown = new RateBreakdown();
            
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

        private async void Insert_Clicked(object sender, EventArgs e)
        {
            Rootobject task = await apiService.GetRate();
            //task.Wait();
            rate = task;

            //call breakdown before inserting
            rateBreakdown.InsertPrimary(db, rate);
            rateBreakdown.InsertSecondary(db, rate);
        }

        private void Select_Clicked(object sender, EventArgs e)
        {
            DatabaseManager db = new DatabaseManager();
            List<tblExchangeRates> rates = db.GetAllRates();
        }
    }
}