using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OpenBr.LaborTaxes.Web.Api.Models;
using System.Linq;

namespace OpenBr.LaborTaxes.Web.Api.Filters
{

    /// <summary>
    /// Validation filter of request models
    /// </summary>
    public class ValidateModelFilter : IActionFilter
    {

        ///<inheritdoc/>
        public void OnActionExecuted(ActionExecutedContext ctx) { }

        ///<inheritdoc/>
        public void OnActionExecuting(ActionExecutingContext ctx)
        {
            if (!ctx.ModelState.IsValid)
            {
                ctx.Result = new BadRequestObjectResult(
                        new ValidationModelResult()
                        {
                            Criticals = ctx.ModelState.Where(x => x.Value.Errors.Count() > 0).ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                        });
            }
        }
    }

}
