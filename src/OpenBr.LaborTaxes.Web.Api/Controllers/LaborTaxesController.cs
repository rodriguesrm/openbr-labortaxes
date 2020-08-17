using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenBr.LaborTaxes.Business.Enums;
using OpenBr.LaborTaxes.Business.Model;
using OpenBr.LaborTaxes.Business.Services;
using OpenBr.LaborTaxes.Web.Api.Models;

namespace OpenBr.LaborTaxes.Web.Api.Controllers
{

    /// <summary>
    /// Inss controller API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LaborTaxesController : ControllerBase
    {

        /// <summary>
        /// Calculate inss rate and values
        /// </summary>
        /// <param name="service">Inss service object</param>
        /// <param name="type">Inss type  (1=Worker, 2=Individual, 3=ManagingPartner)</param>
        /// <param name="revenue">Total revenue to base calculate</param>
        /// <param name="date">Reference date for calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        /// <response code="200">Operation sucess, return response data</response>
        /// <response code="400">Bad request, see details</response>
        [HttpGet("inss/{type:int}/{revenue:decimal}")]
        [ProducesResponseType(typeof(CalculateInssResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationModelResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateInss
        (
            [FromServices] ILaborTaxesService service,
            [RegularExpression("(^[1-3]{1}$)", ErrorMessage = "Invalid type")] [FromRoute] int type,
            [FromRoute] decimal revenue,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken = default
        )
        {
            var resp = await service.CalculateInss((InssType)type, revenue, date, cancellationToken);
            if (resp == null)
                return NotFound(resp);
            return Ok(resp);
        }

    }
}
