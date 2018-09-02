using System.Collections.Generic;
using FrequencyCalculationService;

namespace FrequencyCalculationServiceTests
{
    internal class TestDataWriter : IDataWriter
    {
        public IDictionary<string, long> Data { get; private set; }

        public void SaveDictionary(IDictionary<string, long> frequencyDictionary)
        {
            Data = frequencyDictionary;
        }
    }
}
