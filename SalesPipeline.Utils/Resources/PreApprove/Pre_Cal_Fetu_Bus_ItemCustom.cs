using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_Cal_Fetu_Bus_ItemCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid Pre_Cal_Fetu_BusId { get; set; }

		/// <summary>
		/// ลำดับ
		/// </summary>
		public int SequenceNo { get; set; }

		public string? Name { get; set; }

		public virtual List<Pre_Cal_Fetu_Bus_Item_ScoreCustom>? Pre_Cal_Fetu_Bus_Item_Scores { get; set; }
	}
}
