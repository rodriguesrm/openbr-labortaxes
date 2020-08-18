using Microsoft.VisualBasic;
using OpenBr.LaborTaxes.Business.Documents;
using OpenBr.LaborTaxes.Business.Enums;
using OpenBr.LaborTaxes.Business.Model;
using OpenBr.LaborTaxes.Business.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.LaborTaxes.Business.Services
{

    /// <summary>
    /// Labor taxes service object
    /// </summary>
    public class LaborTaxesService : ILaborTaxesService
    {

        #region Local objects/variables

        private readonly IInssRepository _inssRepository;
        private readonly IIrpfRepository _irpfRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new service instance
        /// </summary>
        public LaborTaxesService(IInssRepository inssRepository, IIrpfRepository irpfRepository)
        {
            _inssRepository = inssRepository;
            _irpfRepository = irpfRepository;
        }

        #endregion

        ///<inheritdoc/>
        public async Task<CalculateInssResult> CalculateInss(InssType type, decimal revenue, DateTime? date, CancellationToken cancellationToken = default)
        {

            CalculateInssResult result = null;

            InssTax inss = await _inssRepository.GetActive(type, date, cancellationToken);
            if (inss != null)
            {
                InssTaxRange range = inss
                    .Range
                    .OrderByDescending(o => o.EndValue)
                    .FirstOrDefault(x => (revenue >= x.StartValue && revenue <= x.EndValue) || revenue >= x.EndValue);

                if (range != null)
                {

                    result = new CalculateInssResult();
                    decimal value = (revenue * range.Rate / 100);
                    value -= range.DeductedAmount;
                    if (value > inss.Limit)
                    {
                        value = inss.Limit;
                        result.IsLimit = true;
                    }
                    value = Math.Round(value, 2);

                    result.Rate = range.Rate;
                    result.Amount = value;
                }

            }

            return result;

        }

        ///<inheritdoc/>
        public async Task<CalculateIrpfResult> CalculateIrpf(decimal revenue, decimal inssValue, byte dependentsNumber, DateTime? date, CancellationToken cancellationToken = default)
        {

            CalculateIrpfResult result = null;

            IrpfTax irpf = await _irpfRepository.GetActive(date, cancellationToken);
            if (irpf != null)
            {

                decimal dependentsDeductionAmount = (dependentsNumber * irpf.DeductionAmount);
                decimal calculationBasis = (revenue - inssValue - dependentsDeductionAmount);

                IrpfTaxRange range = irpf
                    .Range
                    .OrderByDescending(o => o.EndValue)
                    .FirstOrDefault(x => (calculationBasis >= x.StartValue && calculationBasis <= x.EndValue) || calculationBasis >= x.EndValue);

                if (range != null)
                {

                    result = new CalculateIrpfResult
                    {
                        CalculationBasis = calculationBasis,
                        Rate = range.Rate,
                        DependentsDeductionAmount = dependentsDeductionAmount
                    };

                    result.Amount = Math.Round(((calculationBasis * (range.Rate / 100)) - range.DeductionAmount), 2);

                }

            }

            return result;

        }

        ///<inheritdoc/>
        public async Task<CalculateNetRevenueResult> CalculateNetRevenue(InssType type, decimal revenue, byte dependentsNumber, DateTime? date, CancellationToken cancellationToken = default)
        {
            CalculateNetRevenueResult result = new CalculateNetRevenueResult()
            {
                Inss = await CalculateInss(type, revenue, date, cancellationToken)
            };
            result.Irpf = await CalculateIrpf(revenue, result.Inss.Amount, dependentsNumber, date, cancellationToken);
            result.NetRevenue = revenue - (result.Inss?.Amount ?? 0M) - (result.Irpf?.Amount ?? 0M);
            return result;
        }

    }
}
