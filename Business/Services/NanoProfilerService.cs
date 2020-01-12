using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.IServices;
using DataAccess.IRepositories;
using EF.Diagnostics.Profiling.Timings;

namespace Business.Services
{
    // 安裝NuGet套件: NanoProfiler
    public class NanoProfilerService : INanoProfilerService
    {
        private readonly INanoProfilerRepository _iNanoProfilerRepository;
        public NanoProfilerService(INanoProfilerRepository iNanoProfilerRepository)
        {
            this._iNanoProfilerRepository = iNanoProfilerRepository;
        }

        /// <summary>
        /// 儲存NanoProfiler監控的資料
        /// </summary>
        /// <param name="session">監控的資料</param>
        /// <returns></returns>
        public async Task SaveAsync(ITimingSession session)
        {
            await this._iNanoProfilerRepository.SaveAsync(session);
        }
    }
}
