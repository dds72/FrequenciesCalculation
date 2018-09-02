using System.Collections.Generic;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Represents data aggregator for dictionaries accumulation.
    /// </summary>
    public interface IDataAggregator
    {
        /// <summary>
        /// Merges results from dictionary.
        /// </summary>
        /// <param name="data">Dictionary to merge.</param>
        void MergeData(IDictionary<string, long> data);

        /// <summary>
        /// Gets accumulated dictionary.
        /// </summary>
        /// <returns>Dictionary with accumulated results.</returns>
        IDictionary<string, long> GetData();
    }
}