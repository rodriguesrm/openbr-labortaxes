using System;
using System.ComponentModel.DataAnnotations;

namespace OpenBr.LaborTaxes.Web.Api.Models
{

    /// <summary>
    /// Calculate net revenue request model
    /// </summary>
    public class CalculateNetRevenueRequest : CalculateInssRequest
    {

        /// <summary>
        /// Number of legal dependents
        /// </summary>
        [Range(minimum: byte.MinValue, maximum: byte.MaxValue, ErrorMessage = "Dependents number must be between 0 and 255")]
        public byte DependentsNumber { get; set; }


    }

}
