using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Thailands
{
	public class InfoBranchCustom : CommonModel
	{
		public int BranchID { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		[Range(1, int.MaxValue, ErrorMessage = "กรุณาระบุข้อมูล")]
		public int ProvinceID { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? BranchCode { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? BranchName { get; set; }

		public string? BranchNameMain { get; set; }

	}
}
