using System.Collections.Generic;

namespace OpenBr.LaborTaxes.Business.Infra.Log
{

    public class LogScopeInfo
    {
        public LogScopeInfo() { }

        public string Text { get; set; }

        public Dictionary<string, object> Properties { get; set; }
    }

}
