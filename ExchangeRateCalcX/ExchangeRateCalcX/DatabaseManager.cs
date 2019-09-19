using ExchangeRateCalcX.API;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExchangeRateCalcX
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
 
        public static List<tblExchangeRates> GetAllRates()
        {
            return dbConnection.Query<tblExchangeRates>("Select * From tblExchangeRates");
        }

        public static ObservableCollection<tblCurrencies> GetAllCurrencies()
        {
            return new ObservableCollection<tblCurrencies>(dbConnection.Query<tblCurrencies>("Select * From tblCurrencies limit 10"));
        }

        public static List<tblCurrencies> DeleteAllCurrencies()
        {
            return dbConnection.Query<tblCurrencies>("Delete from tblCurrencies");
        }

        public static int VerifyRecentRateOnDB(tblExchangeRates newRate) {

            string rateFrom = newRate.strRateFrom;
            string rateTo = newRate.strRateTo;

            var recentRate = dbConnection.Query<tblExchangeRates>("SELECT EXISTS(Select * From tblExchangeRates where strRateFrom = \'" + rateFrom + "\' and strRateTo = \'" + rateTo + "\' and dtInserted < \'" + DateTime.Now.AddHours(-6) + "\')");
            
            return recentRate.Count;
        }

        public static void InsertCurrency(tblCurrencies newCurrency)
        {
            dbConnection.Insert(newCurrency);
        }

        public static void InsertRate(tblExchangeRates newRate)
        {
            if (VerifyRecentRateOnDB(newRate)==0)
            {
                //insert if there is no rate < 6 hrs old
                dbConnection.Insert(newRate);
            }
        }

        public static bool VerifyIfListOfCurrenciesExists()
        {
            //Do a select to find if we have a list of rates
            bool exists = false;

            Boolean.TryParse(dbConnection.Query<tblCurrencies>("SELECT EXISTS(Select * From tblCurrencies)").ToString(), out exists);

            return exists;
            
        }        
        public static List<tblExchangeRates> ReadRate(string fromCurrency, string toCurrency)
        {
            return dbConnection.Query<tblExchangeRates>("Select * From tblExchangeRates where strRateFrom = \'" + fromCurrency + "\' and strRateTo = \'" + toCurrency + "\' ORDER BY dtInserted desc limit 1");
        }


        public static void Read() {
	        var query = dbConnection.Table<tblExchangeRates>();

            foreach (var tblExchangeRates in query)
	            Console.WriteLine("Stock: " + tblExchangeRates.strRateFrom);
        }
    }
}
