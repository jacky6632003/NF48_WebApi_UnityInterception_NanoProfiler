using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using EF.Diagnostics.Profiling;
using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace WebApplication.InterceptionBehaviors
{
    public class NanoProfilerInterceptionBehavior : IInterceptionBehavior
    {
        /// <summary>
        /// 回傳true，表示該攔截器會執行，回傳false，將不會執行
        /// </summary>
        public bool WillExecute => true;

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return new Type[] { };
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
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