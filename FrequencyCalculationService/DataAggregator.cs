using System.Collections.Generic;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Accumulates dictionaries in one sorted dictionary.
    /// This implementation is not thread safe.
    /// </summary>
    public class DataAggregator : IDataAggregator
    {
        #region private
        private SortedDictionary<string, long> aggregatedData = new SortedDictionary<string, long>();
        #endregion

        /// <summary>
        /// Merges dictionary to internal sorted dictionary.
        /// </summary>
        /// <param name="data">Dictionary to merge.</param>
        public void MergeData(IDictionary<string, long> data)
        {
            foreach (KeyValuePair<string, long> dataPair in data)
            {
                if (aggregatedData.ContainsKey(dataPair.Key))
                {
                    aggregatedData[dataPair.Key] += dataPair.Value;
                }
                else
                {
                    aggregatedData[dataPair.Key] = dataPair.Value;
                }
            }
        }

        /// <summary>
        /// Gets accumulated dictionary.
        /// </summary>
        /// <returns>Dictionary with accumulated results.</returns>
        public IDictionary<string, long> GetData()
        {
            var clonedData = new Dictionary<string, long>();

            foreach (KeyValuePair<string, long> dataPair in aggregatedData)
            {
                clonedData.Add(dataPair.Key, dataPair.Value);
            }

            return clonedData;
        }
    }
}
