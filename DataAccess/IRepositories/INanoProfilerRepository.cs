using EF.Diagnostics.Profiling.Timings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface INanoProfilerRepository
    {
        /// <summary>
        /// 儲存NanoProfiler監控的資料
        /// </summary>
        /// <param name="session">監控的資料</param>
        /// <returns></returns>
        Task SaveAsync(ITimingSession session);
    }
}
