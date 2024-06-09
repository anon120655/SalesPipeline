using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
	public class PrePayTypeIdModel
	{
		/// <summary>
		/// อัตราดอกเบี้ยคงที่
		/// </summary>
		public static readonly Guid PayType1 = new Guid("6b7e120f-138a-11ef-93fa-30e37aef72fb");
		/// <summary>
		/// อัตราดอกเบี้ยคงที่ตามรอบเวลา
		/// </summary>
		public static readonly Guid PayType2 = new Guid("753e6f06-138a-11ef-93fa-30e37aef72fb");
	}
}
