namespace OpenBr.LaborTaxes.Contract.Model
{

    /// <summary>
    /// Inss operation result model
    /// </summary>
    public class CalculateIrpfResult
    {

        /// <summary>
        /// Tax calculation basis
        /// </summary>
        public decimal CalculationBasis { get; set; }

        /// <summary>
        /// Irpf tax rate
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Irpf tax amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Irpf total deduction amount
        /// </summary>
        public decimal DependentsDeductionAmount { get; set; }

    }
}
