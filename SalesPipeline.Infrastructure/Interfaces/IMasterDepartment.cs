using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IMasterDepartment
	{
		Task<Master_DepartmentCustom> Create(Master_DepartmentCustom model);
		Task<Master_DepartmentCustom> Update(Master_DepartmentCustom model);
		Task DeleteById(UpdateModel model);
		Task<Master_DepartmentCustom> GetById(Guid id);
		Task<string?> GetNameById(Guid id);
		Task<PaginationView<List<Master_DepartmentCustom>>> GetList(allFilter model);
	}
}
