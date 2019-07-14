﻿using ExchangeRateCalcX.API;
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
 
        public int InsertRate(tblExchangeRates newRate)
        {
            return dbConnection.Insert(newRate);            
        }

        public void Read() {
	        var query = dbConnection.Table<tblExchangeRates>();

            foreach (var tblExchangeRates in query)
	            Console.WriteLine("Stock: " + tblExchangeRates.strRateFrom);
        }
    }
}
