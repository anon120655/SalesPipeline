using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface ICustomers
	{
		Task Validate(CustomerCustom model, bool isThrow = false);
		Task<List<CustomerCustom>> ValidateUpload(List<CustomerCustom> model);
		Task<ResponseDefaultModel> VerifyByNumber(string juristicNumber);
		Task<CustomerCustom> Create(CustomerCustom model);
		Task<CustomerCustom> Update(CustomerCustom model);
		Task DeleteById(UpdateModel model);
		Task UpdateStatusById(UpdateModel model);
		Task<CustomerCustom> GetById(Guid id);
		Task<string?> GetCompanyNameById(Guid id);
		Task<PaginationView<List<CustomerCustom>>> GetList(allFilter model);
	}
}
