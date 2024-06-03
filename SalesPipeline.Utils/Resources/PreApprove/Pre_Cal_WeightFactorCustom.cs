using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class Pre_Cal_WeightFactorCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid Pre_CalId { get; set; }

		/// <summary>
		/// 1=ข้อมูลการขอสินเชื่อ
		/// 2=คุณสมบัติมารตฐาน
		/// 3=คุณสมบัติตามประเภทผู้ขอ
		/// 4=คุณสมบัติตามประเภทธุรกิจ
		/// </summary>
		public short Type { get; set; }

		public decimal TotalPercent { get; set; }

		public virtual List<Pre_Cal_WeightFactor_ItemCustom>? Pre_Cal_WeightFactor_Items { get; set; }

		//Custom
		public decimal _TotalPercent
		{
			get
			{
				if (Pre_Cal_WeightFactor_Items?.Count > 0)
				{
					return Pre_Cal_WeightFactor_Items.Sum(x => x.Percent);
				}
				return _TotalPercent;
			}
		}

	}
}
