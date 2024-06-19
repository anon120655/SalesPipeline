using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_CreditScoreCustom : CommonModel
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

		public string? Level { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public int? CreditScore { get; set; }

		public string? Grade { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? LimitMultiplier { get; set; }

		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? RateMultiplier { get; set; }

		public string? CreditScoreColor { get; set; }
	}
}
