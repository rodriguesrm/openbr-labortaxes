using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenBr.Inss.Business.Services;
using OpenBr.Inss.Web.Api.Model;

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
        /// <param name="request">Requet data</param>
        /// <param name="cancellationToken">Operation cancelalation token</param>
        /// <response code="200">Operation sucess, return response data</response>
        /// <response code="400">Bad request, see details</response>
        [HttpPost]
        [ProducesResponseType(typeof(RetirementCalculateRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalculateRetirement
        (
            [FromServices] IRetirementService service,
            [FromBody] RetirementCalculateRequest request,
            CancellationToken cancellationToken = default
        )
        {
            var resp = await service.CalculateRetirement(request.Type, request.TotalRevenue, cancellationToken);
            if (resp == null)
                return NotFound(resp);
            return Ok(resp);
        }

    }
}
