using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateCalcX.Calculator
{
    public class CalculatorClass : ICalculator
    {
        public string Convert(string valueFrom, string rate)
        {
            decimal dmlValueFrom = 0;
            decimal dmlRate = 0;

            if(Decimal.TryParse(valueFrom, out dmlValueFrom))
            {
                if (Decimal.TryParse(rate, out dmlRate))
                {
                    return (dmlValueFrom * dmlRate).ToString("#.##");
                }
            }
            return "0.0";
        }
        public string ConcatenateNumbers(string valueFrom, string valueToConcat)
        {
            return valueFrom + valueToConcat;
        }
    }
}
