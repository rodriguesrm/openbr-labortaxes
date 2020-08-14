using OpenBr.Inss.Business.Enums;
using System.ComponentModel.DataAnnotations;

namespace OpenBr.Inss.Web.Api.Model
{

    /// <summary>
    /// Retirement calculate request model
    /// </summary>
    public class RetirementCalculateRequest
    {

        /// <summary>
        /// Retirement type
        /// </summary>
        /// <remarks>REMARKS</remarks>
        [Required]
        public RetirementType Type { get; set; }

        /// <summary>
        /// Total revenue value
        /// </summary>
        /// <example>5432.87</example>
        [Required]
        public decimal TotalRevenue { get; set; }

    }
}



/// <example>1 //Worker</example>
/// <example>2 //Individual</example>
/// <example>3 //AdminPartner</example>