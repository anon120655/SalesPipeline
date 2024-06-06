using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_Factor_BuCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid Pre_FactorId { get; set; }

		public Guid? Pre_Cal_Fetu_Bus_ItemId { get; set; }

		public string? Pre_Cal_Fetu_Bus_ItemName { get; set; }

		public Guid? Pre_Cal_Fetu_Bus_Item_ScoreId { get; set; }

		public string? Pre_Cal_Fetu_Bus_Item_ScoreName { get; set; }

	}
}
