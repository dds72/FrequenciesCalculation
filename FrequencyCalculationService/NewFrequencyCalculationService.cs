using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Calculates frequency of words in input text data
    /// and outputs sorted dictionary with calculated data.
    /// This implementation is using multiple threads
    /// to calculate words frequencies and updates ConcurrentDictionary.
    /// </summary>
    public class NewFrequencyCalculationService : ICalculationService
    {
        #region private
        private const int MaxBufferSize = 10;

        private readonly IDataReader dataReader;
        private readonly IDataWriter dataWriter;
        private readonly IFrequencyCalculatorWithUpdater frequencyCalculator;
        #endregion

        /// <summary>
        /// Constructs instance of <see cref="FrequencyCalculationService"/>.
        /// </summary>
        /// <param name="dataReader">Instance of <see cref="IDataReader"/> to read input data.</param>
        /// <param name="dataWriter">Instance of <see cref="IDataWriter"/> to write output data.</param>
        /// <param name="frequencyCalculator">Instance of <see cref="IFrequencyCalculator"/> to calculate frequencies.</param>
        public NewFrequencyCalculationService(
            IDataReader dataReader,
            IDataWriter dataWriter,
            IFrequencyCalculatorWithUpdater frequencyCalculator)
        {
            this.dataReader = dataReader;
            this.dataWriter = dataWriter;
            this.frequencyCalculator = frequencyCalculator;
        }

        public void RunCalculation()
        {

        }

        /// <summary>
        /// Runs calculation.
        /// </summary>
        public async Task RunCalculationAsync()
        {
            IDictionary<string, long> resultDictionary = new ConcurrentDictionary<string, long>();

            var extractBlockOptions = new DataflowBlockOptions
            {
                BoundedCapacity = 1
            };

            var calculationBlockOptions = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = Environment.ProcessorCount * MaxBufferSize * 100,
                MaxDegreeOfParallelism = Environment.ProcessorCount * 100
            };

            var linkOptions = new DataflowLinkOptions
            {
                PropagateCompletion = true
            };

            var getBlocks = new BufferBlock<string>(extractBlockOptions);

            var calculateFrequencies = new ActionBlock<string>(
                (text) =>
                {
                    frequencyCalculator.CalculateFrequencies(text, resultDictionary);
                },
                calculationBlockOptions);

            getBlocks.LinkTo(calculateFrequencies, linkOptions);

            string buffer = dataReader.GetBlock();
            while (buffer != null)
            {
                await getBlocks.SendAsync(buffer);
                buffer = dataReader.GetBlock();
            }
            getBlocks.Complete();

            await Task.WhenAll(calculateFrequencies.Completion);

            dataWriter.SaveDictionary(new SortedDictionary<string, long>(resultDictionary));
        }
    }
}
