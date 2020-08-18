using System;
using System.Collections.Generic;
using System.Text;

namespace OpenBr.LaborTaxes.Business.Infra.Log
{
    public class LogExceptionInfo
    {

        public LogExceptionInfo(Exception exception)
        {
            HResult = exception.HResult;
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            Source = exception.Source;

            if (exception.InnerException != null)
                InnerException = new LogExceptionInfo(exception.InnerException);
        }

        public int HResult { get; set; }
        
        public virtual string Message { get; }
        
        public virtual string StackTrace { get; }
        
        public virtual string Source { get; set; }

        public LogExceptionInfo InnerException { get; set; }

    }

}
