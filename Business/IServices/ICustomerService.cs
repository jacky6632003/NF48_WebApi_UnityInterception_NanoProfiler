using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace Business.IServices
{
    public interface ICustomerService
    {
        /// <summary>
        /// 取得客戶清單
        /// </summary>
        /// <param name="customerId">客戶編號(多筆)</param>
        /// <returns></returns>
        Task<IEnumerable<CustomerModel>> GetCustomerListAsync(IEnumerable<string> customerId);
    }
}
