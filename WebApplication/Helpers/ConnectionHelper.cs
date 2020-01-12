using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DataAccess.Helpers;
using EF.Diagnostics.Profiling;
using EF.Diagnostics.Profiling.Data;

namespace WebApplication.Helpers
{
    public class ConnectionHelper : IConnectionHelper
    {
        private readonly IConnectionStringHelper _connectionStringHelper;
        private readonly string _northwindDbName = "Northwind";

        public ConnectionHelper(IConnectionStringHelper connectionStringHelper)
        {
            this._connectionStringHelper = connectionStringHelper;
        }

        /// <summary>
        /// Get Drama Connection
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetNorthwindConnection(bool isNanoProflerConn = true)
        {
            return this.GetConnection(this._northwindDbName, isNanoProflerConn);
        }

        /// <summary>
        /// Get GetConnection
        /// </summary>
        /// <param name="dbName">Db Name</param>
        /// <returns></returns>
        private IDbConnection GetConnection(string dbName, bool isNanoProflerConn = true)
        {
            var connectionString = this._connectionStringHelper.GetConnectionString(dbName);

            var connection = new SqlConnection(connectionString);

            //如果web.config的nanoprofiler circularBufferSize="0"，CircularBuffer就為null
            //表示不保留NanoProfiler的紀錄，可以測試機circularBufferSize有數值，正式就為0
            if (ProfilingSession.CircularBuffer == null || isNanoProflerConn == false)
            {
                //一般的connection
                return connection;
            }

            //NanoProfiller的connection (可以監控到TSQL與傳入的參數)
            var dbProfiler = new DbProfiler(ProfilingSession.Current.Profiler);
            return new ProfiledDbConnection(connection, dbProfiler);
        }
    }
}