using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface ISystemRepo
	{
		Task<System_SignatureCustom> CreateSignature(System_SignatureCustom model);
		Task<System_SignatureCustom> GetSignatureLast(int userid);
		Task<System_SLACustom> CreateSLA(System_SLACustom model);
		Task<System_SLACustom> UpdateSLA(System_SLACustom model);
		Task DeleteSLAById(UpdateModel model);
		Task<System_SLACustom> GetSLAById(Guid id);
		Task<PaginationView<List<System_SLACustom>>> GetListSLA(allFilter model);
		Task<List<System_ConfigCustom>> GetConfig();
		Task<System_ConfigCustom?> GetConfigByCode(string code);
		Task UpdateConfig(List<System_ConfigCustom> model);
        Task ClearDatabase(string code);
    }
}
