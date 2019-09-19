using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ExchangeRateCalcX.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ExchangeRateCalcX.API.Rootobject;

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
            Rootobject rate = new Rootobject();

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
            Currency.Rootobject currency = new Currency.Rootobject();

            //Act
            currency = await apiService.GetListOfCurrenciesFromAPI();

            //Assert
            Assert.IsNotNull(currency);
        }
    }
}
