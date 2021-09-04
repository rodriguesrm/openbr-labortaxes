namespace OpenBr.LaborTaxes.Contract.Model
{

    /// <summary>
    /// Net revenue operation result model
    /// </summary>
    public class CalculateNetRevenueResult
    {

        /// <summary>
        /// Inss operation result data
        /// </summary>
        public CalculateInssResult Inss { get; set; }

        /// <summary>
        /// Irpf operation result data
        /// </summary>
        public CalculateIrpfResult Irpf { get; set; }


        /// <summary>
        /// Net revenue value
        /// </summary>
        public decimal NetRevenue { get; set; }

    }
}
