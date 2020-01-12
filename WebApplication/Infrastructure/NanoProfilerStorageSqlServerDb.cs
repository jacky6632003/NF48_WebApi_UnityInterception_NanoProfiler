using Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Business.IServices;
using Common.Helpers;
using DataAccess.Repositories;
using EF.Diagnostics.Profiling.Storages;
using EF.Diagnostics.Profiling.Timings;
using WebApplication.Helpers;

namespace WebApplication.Infrastructure
{
    /*
    把監控的儲到SQL Server:
    1.web.config 加上 <nanoprofiler circularBufferSize="200" storage="WebApplication.Infrastructure.NanoProfilerStorageSqlServerDb, WebApplication" />
      storage對應到 專案的namespace、專案名稱
    2.建立 NanoProfilerStorageSqlServerDb
    3.建立 NanoProfilerService、NanoProfilerRepository的介面與實作
    4.新增 Table: NanoprofilerLog
    */
    public class NanoProfilerStorageSqlServerDb : ProfilingStorageBase
    {
        private readonly INanoProfilerService _iNanoProfilerService;
        public NanoProfilerStorageSqlServerDb()
        {
            var service = new NanoProfilerService(new NanoProfilerRepository(new ConnectionHelper(new ConnectionStringHelper())));
            this._iNanoProfilerService = service;
        }

        protected override void Save(ITimingSession session)
        {
            //this._iNanoProfilerService.Save(session); //射後不理

            //this._iNanoProfilerService.Save(session).Wait();

            AsyncHelper.RunSync(() => this._iNanoProfilerService.SaveAsync(session));
        }
    }
}