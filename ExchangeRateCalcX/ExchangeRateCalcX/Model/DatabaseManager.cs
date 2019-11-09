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
        public static SQLiteAsyncConnection dbConnection;
        static DatabaseManager()
        {
            if (dbConnection == null)
            {
                dbConnection = DependencyService.Get<IDBInterface>().CreateConnection();
            }
        }

        public static SQLiteAsyncConnection GetConnection()
        {
            if (dbConnection == null)
            {
                dbConnection = DependencyService.Get<IDBInterface>().CreateConnection();
            }
            return dbConnection;
        }
 
        public static async Task<List<Rate>> GetAllRates()
        {
            var query =  dbConnection.Table<Rate>();//("Select * From Rate");
            var result = await query.ToListAsync();
            return result;
        }

        public static async Task<ObservableCollection<Currency>> GetAllCurrencies()
        {
            var query = dbConnection.Table<Currency>();
            var result = await query.ToListAsync();

            return new ObservableCollection<Currency>(result as List<Currency>); ; //Query<CurrencyToken.Rootobject>("Select * From Currency limit 10"));
        }

        public static async void DeleteAllCurrencies()
        {
            //return dbConnection.Query<CurrencyToken>("Delete from Currency");
            await dbConnection.DeleteAllAsync<Currency>();
        }

        public static async Task<int> VerifyRecentRateOnDB(Rate newRate) {

            string rateFrom = newRate.fr;
            string rateTo = newRate.to;

            var recentRate = dbConnection.Table<Rate>().Where(r => r.fr.Equals(rateFrom) && r.to.Equals(rateTo)).OrderByDescending(r=>r.dtInserted).Take(1);  //&& DateTime.Parse(r.dtInserted) < DateTime.Now.AddHours(-6)

            var result = await recentRate.FirstOrDefaultAsync();

            if (result == null)
            {
                return 0;
            }

            if (DateTime.Parse(result.dtInserted) < DateTime.Now.AddHours(-6) && DateTime.Parse(result.dtInserted) > DateTime.MinValue)
            {
                return 1;
            }
            return 0;
        }

        public static async Task InsertCurrency(Currency newCurrency)
        {
            await dbConnection.InsertAsync(newCurrency);
        }

        public static async void InsertRate(Rate newRate)
        {
            var rowsRetrieved = await VerifyRecentRateOnDB(newRate);

            if (rowsRetrieved==0)
            {
                //insert if there is no rate < 6 hrs old
                await dbConnection.InsertAsync(newRate);
            }
        }
              
        public static async Task<List<Rate>> ReadRate(string fromCurrency, string toCurrency)
        {
            var query = dbConnection.Table<Rate>().Where(r => r.fr.Equals(fromCurrency) && r.to.Equals(toCurrency)).OrderByDescending(r => r.dtInserted).Take(1); //.Query<Rate>("Select * From Rate where strRateFrom = \'" + fromCurrency + "\' and strRateTo = \'" + toCurrency + "\' ORDER BY dtInserted desc limit 1");

            var result = await query.ToListAsync();

            return result;
        }

        public static async Task<bool> VerifyIfListOfCurrenciesExists()
        {
            //Do a select to find if we have a list of rates
            bool exists = false;

            var query = dbConnection.Table<Currency>();

            var result = await query.ToListAsync();

            if (result.Count == 1)
                exists = true;

            return exists;

        }
    }
}
