using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Thailands
{
	public class InfoProvinceCustom
	{
		public int ProvinceID { get; set; }

		public string ProvinceCode { get; set; } = null!;

		public string ProvinceName { get; set; } = null!;

		public int? RegionID { get; set; }

		/// <summary>
		/// กิจการสาขาภาค
		/// </summary>
		public Guid? Master_Department_BranchId { get; set; }

	}
}
