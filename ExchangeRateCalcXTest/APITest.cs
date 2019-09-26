using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ExchangeRateCalcX.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateCalcXTest
{
    [TestClass]
    public class APITest
    {
        [TestMethod]
        public async Task GetRateTestValid()
        {
            //Arrange
            APIService apiService = new APIService();
            RateToken.Rootobject rate = new RateToken.Rootobject();

            //Act
            rate = await apiService.GetRate("ALL","XCD");

            //Assert
            Assert.IsNotNull(rate);
        }

        [TestMethod]
        public async Task GetListOfCurrenciesFromAPITest()
        {
            //Arrange
            APIService apiService = new APIService();
            CurrencyToken.Rootobject currency = new CurrencyToken.Rootobject();

            //Act
            currency = await apiService.GetListOfCurrenciesFromAPI();

            //Assert
            Assert.IsNotNull(currency);
        }
    }
}
