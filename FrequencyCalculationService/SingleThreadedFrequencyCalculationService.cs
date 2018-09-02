namespace FrequencyCalculationService
{
    /// <summary>
    /// Calculates frequency of words in input text data
    /// and outputs sorted dictionary with calculated data.
    /// This implementation is using signle thread
    /// to calculate words frequencies.
    /// </summary>
    public class SingleThreadedFrequencyCalculationService
    {
        #region private
        private readonly IDataReader dataReader;
        private readonly IDataWriter dataWriter;
        private readonly IDataAggregator dataAggregator;
        private readonly IFrequencyCalculator frequencyCalculator;
        #endregion

        /// <summary>
        /// Constructs instance of <see cref="FrequencyCalculationService"/>.
        /// </summary>
        /// <param name="dataReader">Instance of <see cref="IDataReader"/> to read input data.</param>
        /// <param name="dataWriter">Instance of <see cref="IDataWriter"/> to write output data.</param>
        /// <param name="dataAggregator">Instance of <see cref="IDataAggregator"/> to aggregate results.</param>
        /// <param name="frequencyCalculator">Instance of <see cref="IFrequencyCalculator"/> to calculate frequencies.</param>
        public SingleThreadedFrequencyCalculationService(
            IDataReader dataReader,
            IDataWriter dataWriter,
            IDataAggregator dataAggregator,
            IFrequencyCalculator frequencyCalculator)
        {
            this.dataReader = dataReader;
            this.dataWriter = dataWriter;
            this.dataAggregator = dataAggregator;
            this.frequencyCalculator = frequencyCalculator;
        }

        public void RunCalculation()
        {
            string buffer = dataReader.GetBlock();
            while (buffer != null)
            {
                dataAggregator.MergeData(
                    frequencyCalculator.CalculateFrequencies(buffer));

                buffer = dataReader.GetBlock();
            }

            dataWriter.SaveDictionary(dataAggregator.GetData());
        }
    }
}
