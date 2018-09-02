using System.Threading.Tasks;

namespace FrequencyCalculationService
{
    /// <summary>
    /// Represents word frequency calculation service.
    /// </summary>
    public interface ICalculationService
    {
        /// <summary>
        /// Runs calculation asynchronously.
        /// </summary>
        Task RunCalculationAsync();
    }
}