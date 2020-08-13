using Newtonsoft.Json.Converters;
using OpenBr.Inss.Business.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        /// <remarks>
        /// Remarks</remarks>
        [Required]
        [EnumDataType(typeof(RetirementType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public RetirementType Type { get; set; }

        /// <summary>
        /// Total revenue value
        /// </summary>
        /// <example>5432.87</example>
        [Required]
        public decimal TotalRevenue { get; set; }

    }
}
