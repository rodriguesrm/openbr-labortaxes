using OpenBr.LaborTaxes.Business.Enums;
using System.ComponentModel.DataAnnotations;

namespace OpenBr.LaborTaxes.Web.Api.Models
{

    /// <summary>
    /// Calculate inss request model
    /// </summary>
    public class CalculateInssRequest
    {

        /// <summary>
        /// Inss type  (1=Worker, 2=Individual, 3=ManagingPartner)
        /// </summary>
        [Required(ErrorMessage = "Type is required")]
        [EnumDataType(typeof(InssType), ErrorMessage = "Invalid type")]
        public InssType? Type { get; set; }

        /// <summary>
        /// Total revenue to base calculate
        /// </summary>
        [Range(minimum: 0D, maximum: 999999.99D, ErrorMessage = "Revenue amount must be between 0.01 and 999999.99")]
        public decimal Revenue { get; set; }

    }

}
