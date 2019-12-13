using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateCalcX.Calculator
{
    public interface ICalculator
    {
        string Convert(string valueFrom, string rate);

        string ConcatenateNumbers(string valueFrom, string valueToConcat);
    }
}
