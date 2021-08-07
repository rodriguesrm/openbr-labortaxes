using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenBr.LaborTaxes.Business.Services;
using OpenBr.LaborTaxes.Contract;
using OpenBr.LaborTaxes.Grpc.Host.Extensions;
using System;
using System.Threading.Tasks;
using InssBusinessType = OpenBr.LaborTaxes.Contract.Enums.InssType;
using RSoft.Logs.Extensions;
using OpenBr.LaborTaxes.Contract.Model;

namespace OpenBr.LaborTaxes.Grpc.Host.Services
{
    public class LaborTaxesGrpcService : Contract.LaborTaxes.LaborTaxesBase
    {

        #region Local objects/variables

        private readonly ILaborTaxesService _laborTaxesService;
        private readonly ILogger<LaborTaxesGrpcService> _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new LaborTaxesService instance
        /// </summary>
        /// <param name="laborTaxesService">ILaborTaxesService bussiness service </param>
        /// <param name="logger">Logger object instance</param>
        public LaborTaxesGrpcService(ILaborTaxesService laborTaxesService, ILogger<LaborTaxesGrpcService> logger) : base()
        {
            _laborTaxesService = laborTaxesService;
            _logger = logger;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Convert grpc inss type to enum inss type
        /// </summary>
        /// <param name="inssType">Grpc inss request type</param>
        private static InssBusinessType ConvertInssType(InssType inssType)
        {
            return inssType switch
            {
                InssType.Worker => InssBusinessType.Worker,
                InssType.Individual => InssBusinessType.Individual,
                InssType.ManagingPartner => InssBusinessType.ManagingPartner,
                _ => InssBusinessType.Individual,
            };
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

            _logger.LogInformation("gRPC LaborTaxesService CalculateInss - START");
            _logger.LogInformation("Request: {Request}", request.AsJson());

            InssBusinessType inssType = ConvertInssType(request.InssType);

            DateTime? date = null;
            if (DateTime.TryParse(request.ReferenceDate, out DateTime dateParsed))
                date = dateParsed;

            CalculateInssReply reply = new();
            try
            {

                decimal revenue = (decimal)request.Revenue;

                CalculateInssResult resp = await _laborTaxesService.CalculateInss(inssType, revenue, date, default);

                if (resp == null)
                {
                    reply.Success = false;
                    reply.Errors = $"400-Bad Request!";
                }
                else
                {
                    reply = resp.MapResult();
                }
                
                _logger.LogInformation("Response: {Response}", reply.AsJson());
                _logger.LogInformation("gRPC LaborTaxesService CalculateInss - END");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                reply.Success = false;
                reply.Errors = ex.Message;
            }

            return reply;
        }

        /// <summary>
        /// Calculate irpf rate and values
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="context">Reply data</param>
        public override async Task<CalculateIrpfReply> CalculateIrpf(CalculateIrpfRequest request, ServerCallContext context)
        {

            _logger.LogInformation("gRPC LaborTaxesService CalculateIrpf - START");
            _logger.LogInformation("Request: {Request}", request.AsJson());

            DateTime? date = null;
            if (DateTime.TryParse(request.ReferenceDate, out DateTime dateParsed))
                date = dateParsed;

            CalculateIrpfReply reply = new();
            try
            {

                decimal revenue = (decimal)request.Revenue;
                decimal inssValue = (decimal)request.InssValue;
                byte dependentsNumber = (byte)request.DependentsNumber;

                CalculateIrpfResult resp = await _laborTaxesService.CalculateIrpf(revenue, inssValue, dependentsNumber, date);

                if (resp == null)
                {
                    reply.Success = false;
                    reply.Errors = $"400-Bad Request!";
                }
                else
                {
                    reply = resp.MapResult();
                }

                _logger.LogInformation("Response: {Response}", reply.AsJson());
                _logger.LogInformation("gRPC LaborTaxesService CalculateIrpf - END");

            }
            catch (Exception ex)
            {   
                _logger.LogError(ex, ex.Message);
                reply.Success = false;
                reply.Errors = ex.Message;
            }

            return reply;

        }

        /// <summary>
        /// Calculate net revenue by deducting inss and irpf
        /// </summary>
        /// <param name="request">Request data</param>
        /// <param name="context">Reply data</param>
        public override async Task<CalculateNetRevenueReply> CalculateNetRevenue(CalculateNetRevenueRequest request, ServerCallContext context)
        {

            _logger.LogInformation("gRPC LaborTaxesService CalculateNetRevenue - START");
            _logger.LogInformation("Request: {Request}", request.AsJson());
            
            InssBusinessType inssType = ConvertInssType(request.InssType);

            DateTime? date = null;
            if (DateTime.TryParse(request.ReferenceDate, out DateTime dateParsed))
                date = dateParsed;

            CalculateNetRevenueReply reply = new();
            try
            {

                decimal revenue = (decimal)request.Revenue;
                byte dependentsNumber = (byte)request.DependentsNumber;

                CalculateNetRevenueResult resp = await _laborTaxesService.CalculateNetRevenue(inssType, revenue, dependentsNumber, date, default);
                if (resp == null)
                {
                    reply.Success = false;
                    reply.Errors = $"400-Bad Request!";
                }
                else
                {
                    reply = resp.MapResult();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                reply.Success = false;
                reply.Errors = ex.Message;
            }

            _logger.LogInformation("Response: {Response}", reply.AsJson());
            _logger.LogInformation("gRPC LaborTaxesService CalculateNetRevenue - END");

            return reply;

        }

        #endregion

    }
}
