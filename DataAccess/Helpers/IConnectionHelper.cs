using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public interface IConnectionHelper
    {
        /// <summary>
        /// Get Northwind Connection
        /// </summary>
        /// <returns></returns>
        IDbConnection GetNorthwindConnection(bool isNanoProflerConn = true);
    }
}
