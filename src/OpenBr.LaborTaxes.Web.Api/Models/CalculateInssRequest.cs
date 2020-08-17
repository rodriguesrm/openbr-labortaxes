using OpenBr.LaborTaxes.Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public decimal Revenue { get; set; }

    }

}
