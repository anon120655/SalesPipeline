using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Thailands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IThailand
	{
		Task<IList<InfoProvinceCustom>> GetProvince(Guid? department_BranchId = null);
		Task<IList<InfoAmphurCustom>> GetAmphur(int provinceID);
		Task<IList<InfoTambolCustom>> GetTambol(int provinceID, int amphurID);
		Task<InfoProvinceCustom> GetProvinceByid(int id);
		Task<InfoAmphurCustom> GetAmphurByid(int id);
		Task<InfoTambolCustom> GetTambolByid(int id);
		Task<string?> GetProvinceNameByid(int id);
		Task<string?> GetAmphurNameByid(int id);
		Task<string?> GetTambolNameByid(int id);
		Task MapZipCode(List<InfoTambolCustom> tambolList);
		Task<InfoBranchCustom> CreateBranch(InfoBranchCustom model);
	}
}
