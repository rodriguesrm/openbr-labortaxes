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
    public class GrpcLaborTaxesProvider : IGrpcLaborTaxesProvider
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

        ///<inheritdoc/>
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
                    _logger?.LogInformation("CalculateInss FAIL, {Errors}", result.Errors);
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors = ex.Message;
                _logger?.LogInformation("CalculateInss FAIL, {Message}", ex.Message);
            }

            return Task.FromResult(result);

        }

        ///<inheritdoc/>
        public Task<CalculateBaseResult<CalculateIrpfResult>> CalculateIrpf(CalculateIrpfArgs args)
        {

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (string.IsNullOrWhiteSpace(_url))
                throw new InvalidOperationException($"The service url '{_url} is null or empty!");

            _logger?.LogInformation("Process CalculateIrpf command");

            using GrpcChannel channel = CreateAuthenticatedChannel(_url);
            LaborTaxesClient client = new LaborTaxesClient(channel);

            CalculateBaseResult<CalculateIrpfResult> result = new CalculateBaseResult<CalculateIrpfResult>();
            try
            {
                CalculateIrpfRequest request = new CalculateIrpfRequest()
                {
                    InssValue = (double)args.InssValue,
                    Revenue = (double)args.Revenue,
                    DependentsNumber = args.DependentsNumber,
                    ReferenceDate = args.ReferenceDate?.ToString("yyyy-MM-dd") ?? string.Empty
                };

                CalculateIrpfReply reply = client.CalculateIrpf(request);
                result.Success = reply.Success;
                if (reply.Success)
                {
                    result.Data = new CalculateIrpfResult()
                    {
                        CalculationBasis = (decimal)reply.Data.CalculationBasis,
                        Amount = (decimal)reply.Data.Amount,
                        Rate = (decimal)reply.Data.Rate
                    };
                    _logger?.LogInformation("CalculateIrpf Success");
                }
                else
                {
                    result.Data = null;
                    result.Errors = reply.Errors;
                    _logger?.LogInformation("CalculateIrpf FAIL, {Errors}", result.Errors);
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors = ex.Message;
                _logger?.LogInformation("CalculateIrpf FAIL, {Message}", ex.Message);
            }

            return Task.FromResult(result);

        }

        ///<inheritdoc/>
        public Task<CalculateBaseResult<CalculateNetRevenueResult>> CalculateNetRevenue(CalculateNetRevenueArgs args)
        {

            if (args == null)
                throw new ArgumentNullException(nameof(args));

            if (string.IsNullOrWhiteSpace(_url))
                throw new InvalidOperationException($"The service url '{_url} is null or empty!");

            _logger?.LogInformation("Process CalculateIrpf command");

            using GrpcChannel channel = CreateAuthenticatedChannel(_url);
            LaborTaxesClient client = new LaborTaxesClient(channel);

            CalculateBaseResult<CalculateNetRevenueResult> result = new CalculateBaseResult<CalculateNetRevenueResult>();
            try
            {
                CalculateNetRevenueRequest request = new CalculateNetRevenueRequest()
                {
                    InssType = MapInssType(args.InssType),
                    Revenue = (double)args.Revenue,
                    DependentsNumber = args.DependentsNumber,
                    ReferenceDate = args.ReferenceDate?.ToString("yyyy-MM-dd") ?? string.Empty
                };

                CalculateNetRevenueReply reply = client.CalculateNetRevenue(request);
                result.Success = reply.Success;
                if (reply.Success)
                {
                    result.Data = new CalculateNetRevenueResult()
                    {
                        Inss = new CalculateInssResult()
                        {
                            Rate = (decimal)reply.Data.Inss.Rate,
                            Amount = (decimal)reply.Data.Inss.Amount,
                            IsLimit = reply.Data.Inss.IsLimit
                        },
                        Irpf = new CalculateIrpfResult()
                        {
                            CalculationBasis = (decimal)reply.Data.Irpf.CalculationBasis,
                            Amount = (decimal)reply.Data.Irpf.Amount,
                            Rate = (decimal)reply.Data.Irpf.Rate
                        }
                    };
                    _logger?.LogInformation("CalculateIrpf Success");
                }
                else
                {
                    result.Data = null;
                    result.Errors = reply.Errors;
                    _logger?.LogInformation("CalculateIrpf FAIL, {Errors}", result.Errors);
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors = ex.Message;
                _logger?.LogInformation("CalculateIrpf FAIL, {Message}", ex.Message);
            }

            return Task.FromResult(result);

        }

        #endregion

    }

}
