using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Calculates frequency of words in input text data
    /// and outputs sorted dictionary with calculated data.
    /// This implementation is using multiple threads
    /// to calculate words frequencies.
    /// </summary>
    public class FrequencyCalculationService : ICalculationService
    {
        #region private
        private const int MaxBufferSize = 10;

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
        public FrequencyCalculationService(
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

        /// <summary>
        /// Runs calculation asynchronously.
        /// </summary>
        public async Task RunCalculationAsync()
        {
            var extractBlockOptions = new DataflowBlockOptions
            {
                BoundedCapacity = 1
            };

            var calculationBlockOptions = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = Environment.ProcessorCount * MaxBufferSize * 100,
                MaxDegreeOfParallelism = Environment.ProcessorCount * 100
            };

            var aggregationBlockOptions = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = Environment.ProcessorCount * MaxBufferSize * 2000,
                MaxDegreeOfParallelism = 1
            };

            var linkOptions = new DataflowLinkOptions
            {
                PropagateCompletion = true
            };

            var getBlocks = new BufferBlock<string>(extractBlockOptions);

            var calculateFrequencies = new TransformBlock<string, IDictionary<string, long>>(
                (text) =>
                {
                    return frequencyCalculator.CalculateFrequencies(text);
                },
                calculationBlockOptions);

            var aggregateResult = new ActionBlock<IDictionary<string, long>>(
                (dictionary) =>
                {
                    dataAggregator.MergeData(dictionary);
                },
                aggregationBlockOptions);

            getBlocks.LinkTo(calculateFrequencies, linkOptions);
            calculateFrequencies.LinkTo(aggregateResult, linkOptions);

            string buffer = dataReader.GetBlock();
            while (buffer != null)
            {
                await getBlocks.SendAsync(buffer);
                buffer = dataReader.GetBlock();
            }
            getBlocks.Complete();

            await Task.WhenAll(calculateFrequencies.Completion, aggregateResult.Completion);

            dataWriter.SaveDictionary(dataAggregator.GetData());
        }
    }
}
