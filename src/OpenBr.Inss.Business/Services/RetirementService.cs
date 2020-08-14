using OpenBr.Inss.Business.Documents;
using OpenBr.Inss.Business.Enums;
using OpenBr.Inss.Business.Model;
using OpenBr.Inss.Business.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBr.Inss.Business.Services
{

    /// <summary>
    /// Retirement service object
    /// </summary>
    public class RetirementService : IRetirementService
    {

        #region Local objects/variables

        private readonly IRetirementRepository _repository;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new service instance
        /// </summary>
        public RetirementService(IRetirementRepository repository)
        {
            _repository = repository;
        }

        #endregion

        ///<inheritdoc/>
        public async Task<CalculateRetirementResult> CalculateRetirement(RetirementType type, decimal revenue, CancellationToken cancellationToken = default)
        {
            CalculateRetirementResult result = new CalculateRetirementResult();

            Retirement retirement = await _repository.GetActive(type, cancellationToken);
            if (retirement != null)
            {

                RetirementRange range = retirement.Range.OrderByDescending(o => o.EndValure).FirstOrDefault(x => x.EndValure >= revenue);
                
                decimal value = (revenue * range.Rate / 100);
                value -= range.DeductedAmount;
                if (value > retirement.Limit)
                    value = retirement.Limit;
                value = Math.Round(value, 2);
                
                result.Rate = range.Rate;
                result.Amount = value;

            }

            return result;
        }
    }
}
