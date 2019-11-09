using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateCalcX
{
    public interface IDBInterface
    {
        SQLiteAsyncConnection CreateConnection();
    }
}
