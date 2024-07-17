using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Masters
{
	public class Master_Pre_Interest_RateTypeCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		public string? Code { get; set; }

		/// <summary>
		/// ชื่อเต็ม
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// อัตราดอกเบี้ย
		/// </summary>
		public decimal? Rate { get; set; }

		//Custom
		public string? OptionValue { get; set; }
	}
}
