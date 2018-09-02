using System.Collections.Generic;

namespace FrequencyCalculationService
{
    public interface IFrequencyCalculatorWithUpdater
    {
        void CalculateFrequencies(string data, IDictionary<string, long> dictionary);
    }
}