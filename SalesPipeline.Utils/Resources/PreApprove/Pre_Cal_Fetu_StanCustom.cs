using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_Cal_Fetu_StanCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid Pre_CalId { get; set; }

		/// <summary>
		/// คะแนนสูงสุด
		/// </summary>
		public int? HighScore { get; set; }

		public virtual List<Pre_Cal_Fetu_Stan_ItemOptionCustom>? Pre_Cal_Fetu_Stan_ItemOptions { get; set; }

		public virtual List<Pre_Cal_Fetu_Stan_ScoreCustom>? Pre_Cal_Fetu_Stan_Scores { get; set; }
	}
}
