using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotAClue
{
    public static class ExceptionExtensionMethods
    {
        public static String ExtractExceptionMessage(this Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;
            return exception.Message;
        }
    }
}
