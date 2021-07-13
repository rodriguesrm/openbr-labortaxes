using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenBr.LaborTaxes.Business.Model;
using OpenBr.LaborTaxes.Business.Services;
using OpenBr.LaborTaxes.Contract;
using OpenBr.LaborTaxes.Grpc.Host.Extensions;
using System;
using System.Threading.Tasks;
using InssBusinessType = OpenBr.LaborTaxes.Business.Enums.InssType;

namespace OpenBr.LaborTaxes.Grpc.Host.Services
{
    public class LaborTaxesService : Contract.LaborTaxes.LaborTaxesBase
    {

        #region Local objects/variables

        private readonly ILaborTaxesService _laborTaxesService;
        private readonly ILogger<LaborTaxesService> _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new LaborTaxesService instance
        /// </summary>
        /// <param name="laborTaxesService">ILaborTaxesService bussiness service </param>
        /// <param name="logger">Logger object instance</param>
        public LaborTaxesService(ILaborTaxesService laborTaxesService, ILogger<LaborTaxesService> logger) : base()
        {
            _laborTaxesService = laborTaxesService;
            _logger = logger;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Calculate inss rate and values
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="context">Reply data</param>
        public override async Task<CalculateInssReply> CalculateInss(CalculateInssRequest request, ServerCallContext context)
        {

            _logger.LogInformation("gRPC LaborTaxesService CalculateInss - START", request);

            InssBusinessType inssType = request.InssType switch
            {
                InssType.Worker => InssBusinessType.Worker,
                InssType.Individual => InssBusinessType.Individual,
                InssType.ManagingPartner => InssBusinessType.ManagingPartner,
                _ => InssBusinessType.Individual,
            };
            decimal revenue = (decimal)request.Revenue;
            
            DateTime? date = null;
            if (DateTime.TryParse(request.ReferenceDate, out DateTime dateParsed))
                date = dateParsed;

            CalculateInssReply reply = new();
            try
            {
                CalculateInssResult resp = await _laborTaxesService.CalculateInss(inssType, revenue, date, default);

                reply = resp.MapResult();
                
                _logger.LogInformation("gRPC LaborTaxesService CalculateInss - END");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "gRPC LaborTaxesService CalculateInss - FAIL");
                reply.Success = false;
                reply.Errors = $"gRPC LaborTaxesService CalculateInss - FAIL - {ex.Message}";
            }

            return reply;
        }

        /// <summary>
        /// Calculate irpf rate and values
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="context">Reply data</param>
        public override Task<CalculateIrpfReply> CalculateIrpf(CalculateIrpfRequest request, ServerCallContext context)
        {

            _logger.LogInformation("gRPC LaborTaxesService CalculateIrpf - START", request);

            _logger.LogInformation("gRPC LaborTaxesService CalculateIrpf - END");

            return base.CalculateIrpf(request, context);
        }

        /// <summary>
        /// Calculate net revenue by deducting inss and irpf
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="context">Reply data</param>
        public override Task<CalculateNetRevenueReply> CalculateNetRevenue(CalculateNetRevenueRequest request, ServerCallContext context)
        {

            _logger.LogInformation("gRPC LaborTaxesService CalculateNetRevenue - START", request);

            _logger.LogInformation("gRPC LaborTaxesService CalculateNetRevenue - END");

            return base.CalculateNetRevenue(request, context);
        }

        #endregion

    }
}
