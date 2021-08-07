using System;

namespace OpenBr.LaborTaxes.Grpc.Client.Contracts
{

    /// <summary>
    /// Calculate inss arguments methods call
    /// </summary>
    public class CalculateIrpfArgs
    {

        /// <summary>
        /// Revenue amount to calculate inss
        /// </summary>
        public decimal Revenue { get; set; }

        /// <summary>
        /// Inss value
        /// </summary>
        public decimal InssValue { get; set; }

        /// <summary>
        /// Dependendes number
        /// </summary>
        public int DependentsNumber { get; set; }

        /// <summary>
        /// Reference date for calculate inss
        /// </summary>
        public DateTime? ReferenceDate { get; set; }

    }
}
