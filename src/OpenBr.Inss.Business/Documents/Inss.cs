using OpenBr.Inss.Business.Enums;
using System;
using System.Collections.Generic;

namespace OpenBr.Inss.Business.Documents
{

    /// <summary>
    /// Contains the INSS table information
    /// </summary>
    public class Inss : DocumentBase
    {

        /// <summary>
        /// Contribution table type
        /// </summary>
        public InssType Type { get; set; }

        /// <summary>
        /// Table Effective Date Start
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Table Effective Date End
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Limit value of table contribution
        /// </summary>
        public decimal Limit { get; set; }

        /// <summary>
        /// Indicates whether the table is active or inactive
        /// </summary>
        public bool Inactive { get; set; }

        /// <summary>
        /// Contribution Range of values
        /// </summary>
        public IEnumerable<InssRange> Range { get; set; }

    }
}
