using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using EF.Diagnostics.Profiling;

namespace WebApplication.Attributes
{
    /*
    public class NanoProfilerActionFilterAttribute : Attribute, IActionFilter
    {
        public bool AllowMultiple => true;

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            Debug.WriteLine($"[NanoProfilerActionAttribute] {continuation.Target} - {continuation.Method.Name} 執行前");

            HttpResponseMessage result;
            using (ProfilingSession.Current.Step($"{continuation.Target} - {continuation.Method.Name}"))
            {
                result = await continuation();
            }

            Debug.WriteLine($"[NanoProfilerActionAttribute] 執行{continuation.Target} - {continuation.Method.Name} 執行後");

            return result;
        }
    }
    */

    /// <summary>
    /// 用來掛在Controller的ActionFilterAttribute
    /// </summary>
    public class NanoProfilerActionFilterAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"[{this.GetType().Name}] {actionContext.ControllerContext.Controller} - {actionContext.ActionDescriptor.ActionName} 執行前");

            using (ProfilingSession.Current.Step($"[{this.GetType().Name}] {actionContext.ControllerContext.Controller} - {actionContext.ActionDescriptor.ActionName} 執行前"))
            {
                return base.OnActionExecutingAsync(actionContext, cancellationToken);
            }
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"[{this.GetType().Name}-] {actionExecutedContext.ActionContext.ControllerContext.Controller} - {actionExecutedContext.ActionContext.ActionDescriptor.ActionName} 執行後");

            using (ProfilingSession.Current.Step($"[{this.GetType().Name}] {actionExecutedContext.ActionContext.ControllerContext.Controller} - {actionExecutedContext.ActionContext.ActionDescriptor.ActionName} 執行後"))
            {
                return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
            }
        }
    }
}