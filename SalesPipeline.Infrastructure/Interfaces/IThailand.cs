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
		Task<IList<InfoProvince>> GetProvince();
		Task<IList<InfoAmphur>> GetAmphur(int provinceID);
		Task<IList<InfoTambol>> GetTambol(int provinceID, int amphurID);
		Task MapZipCode(List<InfoTambolCustom> tambolList);
	}
}
