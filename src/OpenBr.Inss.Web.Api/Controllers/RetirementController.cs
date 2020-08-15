using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenBr.Inss.Business.Enums;
using OpenBr.Inss.Business.Model;
using OpenBr.Inss.Business.Services;
using OpenBr.Inss.Web.Api.Models;

namespace OpenBr.Inss.Web.Api.Controllers
{

    /// <summary>
    /// Retirement controller API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RetirementController : ControllerBase
    {

        /// <summary>
        /// Calculate retirement rate and values
        /// </summary>
        /// <param name="service">Retirement service object</param>
        /// <param name="type">Retirement type  (1=Worker, 2=Individual, 3=ManagingPartner)</param>
        /// <param name="revenue">Total revenue to base calculate</param>
        /// <param name="date">Reference date for calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        /// <response code="200">Operation sucess, return response data</response>
        /// <response code="400">Bad request, see details</response>
        [HttpGet("{type:int}/{revenue:decimal}")]
        [ProducesResponseType(typeof(CalculateRetirementResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationModelResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateRetirement
        (
            [FromServices] IRetirementService service,
            [RegularExpression("(^[1-3]{1}$)", ErrorMessage = "Invalid type")] [FromRoute] int type,
            [FromRoute] decimal revenue,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken = default
        )
        {
            var resp = await service.CalculateRetirement((RetirementType)type, revenue, date, cancellationToken);
            if (resp == null)
                return NotFound(resp);
            return Ok(resp);
        }

    }
}
