using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMaster_Pre_PayType
	{
		Task<string?> GetNameById(Guid id);
	}
}
