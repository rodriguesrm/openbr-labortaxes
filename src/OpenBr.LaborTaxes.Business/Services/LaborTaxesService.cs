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

        private readonly IInssRepository _repository;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new service instance
        /// </summary>
        public LaborTaxesService(IInssRepository repository)
        {
            _repository = repository;
        }

        #endregion

        ///<inheritdoc/>
        public async Task<CalculateInssResult> CalculateInss(InssType type, decimal revenue, DateTime? date, CancellationToken cancellationToken = default)
        {

            CalculateInssResult result = null;

            InssTax inss = await _repository.GetActive(type, date, cancellationToken);
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
    }
}
