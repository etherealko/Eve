using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace eth.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ThrowInner(this Exception ex)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
        }
    }
}
