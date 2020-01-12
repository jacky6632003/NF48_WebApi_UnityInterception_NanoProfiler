using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public interface IConnectionStringHelper
    {
        /// <summary>
        /// 從web.config抓連線字串 (依照name)
        /// </summary>
        /// <param name="name">connectionStrings節點內Add的name</param>
        /// <returns></returns>
        string GetConnectionString(string name);
    }
}
