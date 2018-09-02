using System.Collections.Generic;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Represents dictionary saver to memory.
    /// </summary>
    public interface IDataWriter
    {
        /// <summary>
        /// Saves dictionary.
        /// </summary>
        /// <param name="frequencyDictionary">Dictionary with data to save.</param>
        void SaveDictionary(IDictionary<string, long> frequencyDictionary);
    }
}
