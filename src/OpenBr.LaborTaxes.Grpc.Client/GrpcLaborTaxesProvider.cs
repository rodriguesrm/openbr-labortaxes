using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenBr.LaborTaxes.Contract;
using OpenBr.LaborTaxes.Contract.Model;
using OpenBr.LaborTaxes.Grpc.Client.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LaborTaxesClient = OpenBr.LaborTaxes.Contract.LaborTaxes.LaborTaxesClient;

namespace OpenBr.LaborTaxes.Grpc.Client
{

    /// <summary>
    /// OpenBr gRpc Labor Taxes provider
    /// </summary>
    public class GrpcLaborTaxesProvider
    {

        #region Local Variables/Objects

        private readonly ILogger<GrpcLaborTaxesProvider> _logger;
        private readonly bool _isProduction;
        private readonly string _url;

        #endregion

        #region Constructors

#if DEBUG

        /// <summary>
        /// Create a new GrpcLaborTaxesProvider instance
        /// </summary>
        public GrpcLaborTaxesProvider() : this(null, null) { }

#endif

        /// <summary>
        /// Create a new GrpcLaborTaxesProvider instance
        /// </summary>
        /// <param name="loggerFactory">Loger factory</param>
        /// <param name="url">Service url</param>
        public GrpcLaborTaxesProvider(ILoggerFactory loggerFactory, string url)
        {
            _logger = loggerFactory?.CreateLogger<GrpcLaborTaxesProvider>();
            _isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production;
            _url = url;
        }

        #endregion

        #region Local methods

        /// <summary>
        /// Map inss type
        /// </summary>
        /// <param name="inssType">Inss type to map</param>
        private InssType MapInssType(Contract.Enums.InssType inssType)
        {
            InssType inssResultType = inssType switch
            {
                Contract.Enums.InssType.Worker => InssType.Worker,
                Contract.Enums.InssType.Individual => InssType.Individual,
                Contract.Enums.InssType.ManagingPartner => InssType.ManagingPartner,
                _ => InssType.Individual,
            };
            return inssResultType;
        }

        /// <summary>
        /// Create authenticated channel to gRpc Service Client
        /// </summary>
        /// <param name="address">Url address</param>
        private GrpcChannel CreateAuthenticatedChannel(string address)
            => CreateAuthenticatedChannel(address, null);

        /// <summary>
        /// Create authenticated channel to gRpc Service Client
        /// </summary>
        /// <param name="address">Url address</param>
        /// <param name="token">Token authentication</param>
        private GrpcChannel CreateAuthenticatedChannel(string address, string token)
        {

            CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    metadata.Add("Authorization", $"Bearer {token}");
                }
                return Task.CompletedTask;
            });


            GrpcChannel channel;
            if (_isProduction)
            {
                channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
                {
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
                });
            }
            else
            {
                channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    }),
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
                });
            }
            return channel;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Call calculate-inss methods
        /// </summary>
        /// <param name="args">Arguments object instance</param>
        public Task<CalculateBaseResult<CalculateInssResult>> CalculateInss(CalculateInssArgs args)
        {

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (string.IsNullOrWhiteSpace(_url))
                throw new InvalidOperationException($"The service url '{_url} is null or empty!");

            _logger?.LogInformation("Process CalculateInss command");

            using GrpcChannel channel = CreateAuthenticatedChannel(_url);
            LaborTaxesClient client = new LaborTaxesClient(channel);

            CalculateBaseResult<CalculateInssResult> result = new CalculateBaseResult<CalculateInssResult>();
            try
            {
                CalculateInssRequest request = new CalculateInssRequest()
                {
                    InssType = MapInssType(args.InssType),
                    Revenue = (double)args.Revenue,
                    ReferenceDate = args.ReferenceDate?.ToString("yyyy-MM-dd") ?? string.Empty
                };

                CalculateInssReply reply = client.CalculateInss(request);
                result.Success = reply.Success;
                if (reply.Success)
                {
                    result.Data = new CalculateInssResult()
                    {
                        Amount = (decimal)reply.Data.Amount,
                        IsLimit = reply.Data.IsLimit,
                        Rate = (decimal)reply.Data.Rate
                    };
                    _logger?.LogInformation("CalculateInss Success");
                }
                else
                {
                    result.Data = null;
                    result.Errors = reply.Errors;
                    _logger?.LogInformation("SendMail FAIL, {Errors}", result.Errors);
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors = ex.Message;
                _logger?.LogInformation("SendMail FAIL, {Message}", ex.Message);
            }

            return Task.FromResult(result);
        }

        #endregion

    }

}
