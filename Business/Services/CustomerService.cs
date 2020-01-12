using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.IServices;
using Common.Attributes;
using Common.Models;
using DataAccess.IRepositories;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _iCustomerRepository;

        public CustomerService(ICustomerRepository iCustomerRepository)
        {
            this._iCustomerRepository = iCustomerRepository;
        }

        /// <summary>
        /// 取得客戶清單
        /// </summary>
        /// <param name="customerId">客戶編號(多筆)</param>
        /// <returns></returns>
        [NanoProfiler] //安裝NuGet套件: Unity.Interception (如果要掛上[NanoProfiler])
        public async Task<IEnumerable<CustomerModel>> GetCustomerListAsync(IEnumerable<string> customerId)
        {
            return await this._iCustomerRepository.GetCustomerListAsync(customerId);
        }
    }
}
