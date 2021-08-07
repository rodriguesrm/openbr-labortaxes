using System;

namespace OpenBr.LaborTaxes.Grpc.Client.Contracts
{

    /// <summary>
    /// Calculate base result
    /// </summary>
    /// <typeparam name="TResultData">Result data type</typeparam>
    public class CalculateBaseResult<TResultData>
    {

        /// <summary>
        /// Success flag
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Result data
        /// </summary>
        public TResultData Data { get; set; } = Activator.CreateInstance<TResultData>();

        /// <summary>
        /// Errors data
        /// </summary>
        public string Errors { get; set; }

    }
}
