using OpenBr.LaborTaxes.Contract.Enums;
using System;

namespace OpenBr.LaborTaxes.Grpc.Client.Contracts
{

    /// <summary>
    /// Calculate netrevenue arguments methods call
    /// </summary>
    public class CalculateNetRevenueArgs
    {

        /// <summary>
        /// Inss type
        /// </summary>
        public InssType InssType { get; set; }

        /// <summary>
        /// Revenue amount to calculate inss
        /// </summary>
        public decimal Revenue { get; set; }

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
