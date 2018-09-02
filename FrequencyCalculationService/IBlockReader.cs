namespace FrequencyCalculationService
{
    /// <summary>
    /// Represents reader which reads blocks from stream with undiveded words.
    /// </summary>
    public interface IBlockReader
    {
        /// <summary>
        /// Gets next block.
        /// </summary>
        /// <returns>Text block.</returns>
        string GetBlock();
    }
}