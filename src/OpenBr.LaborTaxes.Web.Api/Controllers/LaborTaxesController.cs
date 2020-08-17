﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            [FromServices] ILaborTaxesService service,
            [FromBody] CalculateInssRequest request,
            [FromQuery] DateTime? date,
            CancellationToken cancellationToken = default
        )
        {
            var resp = await service.CalculateInss(request.Type.Value, request.Revenue, date, cancellationToken);
            if (resp == null)
                return NotFound(resp);
            return Ok(resp);
        }

    }
}
