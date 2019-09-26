using ExchangeRateCalcX;
using ExchangeRateCalcX.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static ExchangeRateCalcX.Model.CurrencyToken;
using static ExchangeRateCalcX.Model.RateToken;

namespace ExchangeRateCalcX.Model
{
    public static class DatabaseManager
    {
        public static SQLiteConnection dbConnection;
        static DatabaseManager()
        {
            if (dbConnection == null)
            {
                dbConnection = DependencyService.Get<IDBInterface>().CreateConnection();
            }
        }

        public static SQLiteConnection GetConnection()
        {
            if (dbConnection == null)
            {
                dbConnection = DependencyService.Get<IDBInterface>().CreateConnection();
            }
            return dbConnection;
        }
 
        public static List<Rate> GetAllRates()
        {
            return dbConnection.Query<Rate>("Select * From tblExchangeRates");
        }

        public static ObservableCollection<CurrencyToken.Rootobject> GetAllCurrencies()
        {
            return new ObservableCollection<CurrencyToken.Rootobject>(dbConnection.Query<CurrencyToken.Rootobject>("Select * From tblCurrencies limit 10"));
        }

        public static List<CurrencyToken> DeleteAllCurrencies()
        {
            return dbConnection.Query<CurrencyToken>("Delete from tblCurrencies");
        }

        public static int VerifyRecentRateOnDB(Rate newRate) {

            string rateFrom = newRate.from;
            string rateTo = newRate.to;

            var recentRate = dbConnection.Query<Rate>("SELECT EXISTS(Select * From tblExchangeRates where strRateFrom = \'" + rateFrom + "\' and strRateTo = \'" + rateTo + "\' and dtInserted < \'" + DateTime.Now.AddHours(-6) + "\')");
            
            return recentRate.Count;
        }

        public static List<Rate> VerifyRecentRateOnDB(string rateFrom, string rateTo) {

            var recentRate = dbConnection.Query<Rate>("SELECT EXISTS(Select * From tblExchangeRates where strRateFrom = \'" + rateFrom + "\' and strRateTo = \'" + rateTo + "\' and dtInserted < \'" + DateTime.Now.AddHours(-6) + "\')");

            return recentRate;
        }

        public static void InsertCurrency(Currency newCurrency)
        {
            dbConnection.Insert(newCurrency);
        }

        public static void InsertRate(Rate newRate)
        {
            if (VerifyRecentRateOnDB(newRate)==0)
            {
                //insert if there is no rate < 6 hrs old
                dbConnection.Insert(newRate);
            }
        }
              
        public static List<Rate> ReadRate(string fromCurrency, string toCurrency)
        {
            return dbConnection.Query<Rate>("Select * From tblExchangeRates where strRateFrom = \'" + fromCurrency + "\' and strRateTo = \'" + toCurrency + "\' ORDER BY dtInserted desc limit 1");
        }

        public static bool VerifyIfListOfCurrenciesExists()
        {
            //Do a select to find if we have a list of rates
            bool exists = false;

            Boolean.TryParse(dbConnection.Query<CurrencyToken>("SELECT EXISTS(Select * From tblCurrencies)").ToString(), out exists);

            return exists;

        }


        public static void Read() {
	        var query = dbConnection.Table<Rate>();

            foreach (var tblExchangeRates in query)
	            Console.WriteLine("Stock: " + tblExchangeRates.from);
        }
    }
}
