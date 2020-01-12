using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Attributes;
using Common.Models;
using Dapper;
using DataAccess.Helpers;
using DataAccess.IRepositories;

namespace DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConnectionHelper _iConnectionHelper;
        public CustomerRepository(IConnectionHelper iConnectionHelper)
        {
            this._iConnectionHelper = iConnectionHelper;
        }

        /// <summary>
        /// 取得客戶清單
        /// </summary>
        /// <param name="customerId">客戶編號(多筆)</param>
        /// <returns></returns>
        [NanoProfiler] //安裝NuGet套件: Unity.Interception (如果要掛上[NanoProfiler])
        public async Task<IEnumerable<CustomerModel>> GetCustomerListAsync(IEnumerable<string> customerId)
        {
            using (var conn = this._iConnectionHelper.GetNorthwindConnection())
            {
                var result = await conn.QueryAsync<CustomerModel>
                    (
                        this.GetCustomerListAsyncSQL(),
                        new { customerId = customerId }
                    );

                return result;
            }
        }

        /// <summary>
        /// 取得客戶清單 SQL
        /// </summary>
        /// <returns></returns>
        private string GetCustomerListAsyncSQL()
        {
            return @"SELECT 
                     [CustomerID], 
                     [CompanyName], 
	                 [ContactName], 
	                 [ContactTitle], 
	                 [Address], 
	                 [City], 
	                 [Region], 
	                 [PostalCode], 
	                 [Country], 
	                 [NUM-3] AS [NUM_3], 
	                 [ALPHA-2] AS [ALPHA_2], 
	                 [ALPHA-3] AS [ALPHA_3], 
	                 [Phone], 
	                 [Fax] 
                     FROM [dbo].[Customers] 
                     WHERE [CustomerID] IN @customerId";
        }
    }
}
