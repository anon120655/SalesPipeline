using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
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
		Task<int?> GetProvinceIdByName(string name);
		Task<int?> GetAmphurIdByName(string name);
		Task<int?> GetTambolIdByName(string name);
		Task MapZipCode(List<InfoTambolCustom> tambolList);
		Task<InfoBranchCustom> CreateBranch(InfoBranchCustom model);
		Task<InfoBranchCustom> UpdateBranch(InfoBranchCustom model);
		Task<IList<InfoBranchCustom>> GetBranch(int provinceID);
		Task<IList<InfoBranchCustom>> GetBranchByDepBranchId(allFilter model);
		Task<InfoBranchCustom?> GetBranchByid(int id);
		Task<string?> GetBranchNameByid(int id);
	}
}
