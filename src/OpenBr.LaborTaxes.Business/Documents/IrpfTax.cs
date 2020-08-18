using System;
using System.Collections.Generic;

namespace OpenBr.LaborTaxes.Business.Documents
{


    /// <summary>
    /// Contains the IRPF table information
    /// </summary>
    public class IrpfTax : DocumentBase
    {

        /// <summary>
        /// Table Effective Date Start
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Table Effective Date End
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Deduction amount per dependent
        /// </summary>
        public decimal DeductionAmount { get; set; }

        /// <summary>
        /// Indicates whether the table is active or inactive
        /// </summary>
        public bool Inactive { get; set; }

        /// <summary>
        /// Tax Range value
        /// </summary>
        public IEnumerable<IrpfTaxRange> Range { get; set; }

    }
}
