using System.Collections.Generic;

namespace FrequencyCalculationService
{
    public interface IFrequencyCalculator
    {
        IDictionary<string, long> CalculateFrequencies(string data);
    }
}