using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Business.IServices;
using Common.Attributes;
using Common.Models;
using WebApplication.Attributes;

namespace WebApplication.Controllers
{
    [RoutePrefix("Customer")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerService _iCustomerService;
        public CustomerController(ICustomerService iCustomerService)
        {
            this._iCustomerService = iCustomerService;
        }

        /*
        1.先在Url輸入: http://localhost:9933/Customer/Get/1 來測試
        2.在Url輸入: http://localhost:9933/NanoProfiler/View 來查看
        */

        /// <summary>
        /// 取得客戶清單
        /// </summary>
        /// <param name="id">編號</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get/{id}")]
        [NanoProfiler] //安裝NuGet套件: Unity.Interception (如果要掛上[NanoProfiler])
        [NanoProfilerActionFilter]
        public async Task<IEnumerable<CustomerModel>> GetCustomerListAsync(int id)
        {
            var customerIds = new string[] { "ALFKI", "ANATR", "ANTON" };
            return await this._iCustomerService.GetCustomerListAsync(customerIds);
        }
    }
}
