using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataAccess.Helpers;
using DataAccess.IRepositories;
using EF.Diagnostics.Profiling.Timings;

namespace DataAccess.Repositories
{
    // 安裝NuGet套件: NanoProfiler
    public class NanoProfilerRepository : INanoProfilerRepository
    {
        private readonly IConnectionHelper _iConnectionHelper;
        public NanoProfilerRepository(IConnectionHelper iConnectionHelper)
        {
            this._iConnectionHelper = iConnectionHelper;
        }

        /// <summary>
        /// 儲存NanoProfiler監控的資料
        /// </summary>
        /// <param name="session">監控的資料</param>
        /// <returns></returns>
        public async Task SaveAsync(ITimingSession session)
        {
            try
            {
                using (var conn = this._iConnectionHelper.GetNorthwindConnection(false))
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        await SaveMainSessionAsync(session, conn, trans); //請見圖示
                        await SaveRootSessionAsync(session, conn, trans); //請見圖示
                        await SaveDbSessionAsync(session, conn, trans); //請見圖示

                        trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SaveMainSessionAsync(ITimingSession session, IDbConnection conn, IDbTransaction trans)
        {
            await conn.ExecuteAsync
            (
                @"INSERT INTO [dbo].[NanoprofilerLog]
                                ([mainId],[sessionId],[machine],[type],[name],[started],[dbdruation],[druation],[requesttype],[clientip],[dbcount])
                          VALUES
                                (@mainId,@sessionId,@machine,@type,@name,@started,@dbdruation,@druation,@requesttype,@clientip,@dbcount)",
                new
                {
                    mainId = session.Id.ToString(),//主id，不重覆
                    sessionId = session.Id.ToString(),//每次api共同的id
                    machine = session.MachineName,//呼叫的電腦名稱
                    type = session.Type,//總共分session和setp和db，這個是session
                    name = session.Name,//api的名字
                    started = session.Started,//開始時間
                    dbdruation = session.Data.FirstOrDefault(x => x.Key == "dbDruation").Value,//db耗時
                    druation = session.DurationMilliseconds,//api的總耗時
                    requesttype = session.Data.FirstOrDefault(x => x.Key == "requestType").Value,//request的方式，以我的例子是web
                    clientip = session.Data.FirstOrDefault(x => x.Key == "clientIp").Value,
                    dbcount = session.Data.FirstOrDefault(x => x.Key == "dbCount").Value//這次api呼叫了多少個連線，如果有兩個sp就會有兩個連線
                },
                trans
            );
        }

        public async Task SaveRootSessionAsync(ITimingSession session, IDbConnection conn, IDbTransaction trans)
        {
            var rootSession = session.Timings.FirstOrDefault(x => x.Type == "step");
            await conn.ExecuteAsync
            (
                @"INSERT INTO [dbo].[NanoprofilerLog]
                                  ([mainId],[sessionId],[machine],[type],[parentId],[name],[druation])
                          VALUES
                                  (@mainId,@sessionId,@machine,@type,@parentId,@name,@druation)",
                new
                {
                    mainId = rootSession.Id.ToString(),
                    sessionid = session.Id.ToString(),
                    machine = session.MachineName,
                    type = rootSession.Type, //總共分session和setp和db，這個是setp
                    parentId = rootSession.ParentId.ToString(), //對應SaveMainSession的mainId
                    name = rootSession.Name, //紀錄root
                    druation = rootSession.DurationMilliseconds //耗時
                },
                trans
            );
        }

        public async Task SaveDbSessionAsync(ITimingSession session, IDbConnection conn, IDbTransaction trans)
        {
            var dbSession = session.Timings.Where(x => x.Type == "db");
            foreach (var item in dbSession)
            {
                await conn.ExecuteAsync
                (
                    @"INSERT INTO [dbo].[NanoprofilerLog]
                                    ([mainId],[sessionId],[parentId],[machine],[type],[name],[started],[druation],[executetype],[parameters])
                              VALUES
                                    (@mainId,@sessionId,@parentId,@machine,@type,@name,@started,@druation,@executetype ,@parameters)",
                    new
                    {
                        mainId = item.Id.ToString(),
                        sessionid = session.Id.ToString(),
                        parentId = item.ParentId.ToString(), //對應SaveMainSession的mainId
                        machine = session.MachineName,
                        type = item.Type,  //總共分session和setp和db，這個是db
                        name = item.Name, //紀錄執行的sql或sp名稱
                        started = item.Started,
                        druation = item.DurationMilliseconds,
                        executetype = item.Data.FirstOrDefault(x => x.Key == "executeType").Value, //查詢或非查詢
                        parameters = item.Data.FirstOrDefault(x => x.Key == "parameters").Value //參數包含型別和丟進去的數值
                    },
                    trans
                );
            }
        }
    }
}
