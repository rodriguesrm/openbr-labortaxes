using System;
using System.Threading;
using System.Threading.Tasks;
using DnsClient.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        /// <param name="logger">Logger provider object</param>
        /// <param name="service">Labor taxes service object</param>
        /// <param name="request">Request data</param>
        /// <param name="date">Reference date for calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        /// <response code="200">Operation sucess, return response data</response>
        /// <response code="400">Bad request, see details</response>
        [HttpPost("inss")]
        [ProducesResponseType(typeof(CalculateInssResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationModelResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateInss
        (
            [FromServices] ILogger<LaborTaxesController> logger,
            [FromServices] ILaborTaxesService service,
            [FromBody] CalculateInssRequest request,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken = default
        )
        {
            CalculateInssResult resp = await service.CalculateInss(request.Type.Value, request.Revenue, date, cancellationToken);
            if (resp == null)
                return NotFound(resp);
            return Ok(resp);
        }

        /// <summary>
        /// Calculate irpf rate and values
        /// </summary>
        /// <param name="service">Labor taxes service object</param>
        /// <param name="request">Request data</param>
        /// <param name="date">Reference date for calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        /// <response code="200">Operation sucess, return response data</response>
        /// <response code="400">Bad request, see details</response>
        [HttpPost("irpf")]
        [ProducesResponseType(typeof(CalculateIrpfResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationModelResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateIrpf
        (
            [FromServices] ILaborTaxesService service,
            [FromBody] CalculateIrpfRequest request,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken = default
        )
        {
            CalculateIrpfResult resp = await service.CalculateIrpf(request.Revenue, request.InssValue, request.DependentsNumber, date, cancellationToken);
            if (resp == null)
                return BadRequest(resp);
            return Ok(resp);
        }

        /// <summary>
        /// Calculate net revenue by deducting inss and irpf
        /// </summary>
        /// <param name="service">Labor taxes service object</param>
        /// <param name="request">Request data</param>
        /// <param name="date">Reference date for calculate</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        /// <response code="200">Operation sucess, return response data</response>
        /// <response code="400">Bad request, see details</response>
        [HttpPost("net-revenue")]
        [ProducesResponseType(typeof(CalculateNetRevenueResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationModelResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateNetRevenue
        (
            [FromServices] ILaborTaxesService service,
            [FromBody] CalculateNetRevenueRequest request,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken = default
        )
        {
            CalculateNetRevenueResult resp = await service.CalculateNetRevenue(request.Type.Value, request.Revenue, request.DependentsNumber, date, cancellationToken);
            throw new Exception("FAILT TEST");
            if (resp == null)
                return BadRequest(resp);
            return Ok(resp);
        }

    }
}
