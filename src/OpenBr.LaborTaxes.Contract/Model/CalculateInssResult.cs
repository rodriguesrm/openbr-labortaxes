using System;

namespace OpenBr.LaborTaxes.Contract.Model
{

    /// <summary>
    /// Inss operation result model
    /// </summary>
    public class CalculateInssResult
    {

        /// <summary>
        /// Inss rate
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Inss amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Indicate value is limite value
        /// </summary>
        public bool IsLimit { get; set; }

    }

}
