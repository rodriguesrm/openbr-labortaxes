using System.Collections.Generic;

namespace OpenBr.Inss.Web.Api.Models
{

    /// <summary>
    /// Validation criticals information result object
    /// </summary>
    public class ValidationModelResult
    {

        /// <summary>
        /// Critical dictionary
        /// </summary>
        public IDictionary<string, string[]> Criticals { get; set; }

    }
}
