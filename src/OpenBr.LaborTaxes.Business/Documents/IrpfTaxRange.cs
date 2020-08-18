using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBr.LaborTaxes.Business.Documents
{

    /// <summary>
    /// Contains details of the tax range
    /// </summary>
    public class IrpfTaxRange
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
        /// Amount to be deducted (for progressive taxes)
        /// </summary>
        public decimal DeductionAmount { get; set; }

    }

}
