using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_FactorCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public Guid SaleId { get; set; }

		public Guid Pre_CalId { get; set; }

		/// <summary>
		/// ชื่อบริษัท
		/// </summary>
		public string? CompanyName { get; set; }

		public virtual List<Pre_Factor_AppCustom>? Pre_Factor_Apps { get; set; }

		public virtual List<Pre_Factor_BuCustom>? Pre_Factor_Bus { get; set; }

		public virtual List<Pre_Factor_InfoCustom>? Pre_Factor_Infos { get; set; }

		public virtual List<Pre_Factor_StanCustom>? Pre_Factor_Stans { get; set; }

		public virtual List<Pre_ResultCustom>? Pre_Results { get; set; }

	}
}
