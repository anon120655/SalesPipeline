using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class MasterBusinessType : IMasterBusinessType
	{
		public Task<Master_BusinessTypeCustom> Create(Master_BusinessTypeCustom model)
		{
			throw new NotImplementedException();
		}

		public Task<Master_BusinessTypeCustom> Update(Master_BusinessTypeCustom model)
		{
			throw new NotImplementedException();
		}

		public Task DeleteById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public Task UpdateStatusById(UpdateModel model)
		{
			throw new NotImplementedException();
		}

		public Task<Master_BusinessTypeCustom> GetById(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<PaginationView<List<Master_BusinessTypeCustom>>> GetList(allFilter model)
		{
			throw new NotImplementedException();
		}

	}
}
