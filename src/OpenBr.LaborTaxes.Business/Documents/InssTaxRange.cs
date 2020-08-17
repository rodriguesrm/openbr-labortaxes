namespace OpenBr.LaborTaxes.Business.Documents
{

    /// <summary>
    /// Contains details of the contribution range
    /// </summary>
    public class InssTaxRange
    {

        /// <summary>
        /// Range start value (revenue)
        /// </summary>
        public decimal StartValue { get; set; }

        /// <summary>
        /// Range end value (revenue)
        /// </summary>
        public decimal EndValue { get; set; }

        /// <summary>
        /// Contribution rate (percentage to be applied)
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Amount to be deducted (for progressive contributions)
        /// </summary>
        public decimal DeductedAmount { get; set; }

    }
}
