using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface ISales
	{
		Task<SaleCustom> Create(SaleCustom model);
		Task<SaleCustom> Update(SaleCustom model);
		Task UpdateStatusOnly(Sale_StatusCustom model);
		Task<SaleCustom> GetById(Guid id);
	}
}
