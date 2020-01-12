using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using DataAccess.Helpers;

namespace WebApplication.Helpers
{
    public class ConnectionStringHelper : IConnectionStringHelper
    {
        /// <summary>
        /// 從web.config抓連線字串 (依照name)
        /// </summary>
        /// <param name="name">connectionStrings節點內Add的name</param>
        /// <returns></returns>
        public string GetConnectionString(string name)
        {
            return WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}