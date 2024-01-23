using SalesPipeline.Infrastructure.Data.Entity;
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
		Task<IList<InfoProvinceCustom>> GetProvince();
		Task<IList<InfoAmphurCustom>> GetAmphur(int provinceID);
		Task<IList<InfoTambolCustom>> GetTambol(int provinceID, int amphurID);
		Task MapZipCode(List<InfoTambolCustom> tambolList);
	}
}
