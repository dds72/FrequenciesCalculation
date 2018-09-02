namespace FrequencyCalculationService
{
    /// <summary>
    /// Represents data reader to read blocks of data.
    /// </summary>
    public interface IDataReader
    {
        /// <summary>
        /// Reads next block of data.
        /// </summary>
        /// <returns>
        /// New block of data.
        /// null, if there is no more blocks to read.
        /// </returns>
        string GetBlock();
    }
}
