using ExchangeRateCalcX.API;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExchangeRateCalcX
{
    public class DatabaseManager
    {
        SQLiteConnection dbConnection;
        public DatabaseManager()
        {
            dbConnection = DependencyService.Get<IDBInterface>().CreateConnection();
        }
 
        public List<tblExchangeRates> GetAllRates()
        {
            return dbConnection.Query<tblExchangeRates>("Select * From tblExchangeRates");
        }

        public bool VerifyIfRateIsValid(tblExchangeRates newRate) {

            string rateFrom = newRate.strRateFrom;
            string rateTo = newRate.strRateTo;

            bool exists = false;

            Boolean.TryParse(dbConnection.Query<tblExchangeRates>("SELECT EXISTS(Select * From tblExchangeRates where strRateFrom = \'" + rateFrom + "\' and strRateTo = \'" + rateTo + "\' and dtInserted < " + DateTime.Now +")").ToString(), out exists);

            return exists;
        }

        public void InsertCurrency(tblCurrencies newCurrency)
        {
            dbConnection.Insert(newCurrency);
        }

        public void InsertRate(tblExchangeRates newRate)
        {
            if (VerifyIfRateIsValid(newRate))
            {
                //retrieve the last rate (at this point we know the last entry is < 6 hrs old
            }
            else
            {
                dbConnection.Insert(newRate);
            }     
        }

        public void VerifyIfListOfCurrenciesExists()
        {

        }

        

        public void Read() {
	        var query = dbConnection.Table<tblExchangeRates>();

            foreach (var tblExchangeRates in query)
	            Console.WriteLine("Stock: " + tblExchangeRates.strRateFrom);
        }
    }
}
