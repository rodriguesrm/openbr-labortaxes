namespace OpenBr.Inss.Business.Model
{

    /// <summary>
    /// Retirement operation result model
    /// </summary>
    public class CalculateRetirementResult
    {

        /// <summary>
        /// Retirement rate
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Retirement amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Indicate value is limite value
        /// </summary>
        public bool IsLimite { get; set; }

    }

}
