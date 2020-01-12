using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF.Diagnostics.Profiling;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Common.CallHandlers
{
    /*
    安裝NuGet套件:
    1.Unity.Interception
    2.NanoProfiler
    */
    public class NanoProfilerCallHandler : ICallHandler
    {
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {

            using (ProfilingSession.Current.Step($"[{this.GetType().Name}] {input.Target} - {input.MethodBase.Name}"))
            {
                Debug.WriteLine($"[{this.GetType().Name}] {input.Target} - {input.MethodBase.Name} 執行前");

                var result = getNext()(input, getNext);

                Debug.WriteLine($"[{this.GetType().Name}] {input.Target} - {input.MethodBase.Name} 執行後");

                return result;
            }
        }
    }
}
