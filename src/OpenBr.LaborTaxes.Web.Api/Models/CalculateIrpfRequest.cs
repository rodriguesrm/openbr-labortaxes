using System.ComponentModel.DataAnnotations;

namespace OpenBr.LaborTaxes.Web.Api.Models
{

    /// <summary>
    /// Calculate irpf request model
    /// </summary>
    public class CalculateIrpfRequest
    {

        /// <summary>
        /// Total revenue to base calculate
        /// </summary>
        [Range(minimum: 0.01D, maximum: 999999.99D, ErrorMessage = "Revenue amount must be between 0.01 and 999999.99")]
        public decimal Revenue { get; set; }

        /// <summary>
        /// Inss contribution amount
        /// </summary>
        [Range(minimum: 0D, maximum: 999999.99D, ErrorMessage = "Inss value must be between 0.00 and 999999.99")]
        public decimal InssValue { get; set; }

        /// <summary>
        /// Number of legal dependents
        /// </summary>
        [Range(minimum: byte.MinValue, maximum: byte.MaxValue, ErrorMessage = "Dependents number must be between 0 and 255")]
        public byte DependentsNumber { get; set; }

    }
}
