using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using EF.Diagnostics.Profiling;
using EF.Diagnostics.Profiling.Timings;

namespace WebApplication
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // 若在 web.config 中有啟用 NanoProfiler 時套用新規則
            if (ProfilingSession.CircularBuffer != null)
            {
                /*
                // 1. 資料上限為 200 筆
                // 2. 排除回應時間小於 1500 ms 的項目
                ProfilingSession.CircularBuffer =
                    new CircularBuffer<ITimingSession>(200, session => session.DurationMilliseconds < 1500);
                */
            }
        }
    }
}
