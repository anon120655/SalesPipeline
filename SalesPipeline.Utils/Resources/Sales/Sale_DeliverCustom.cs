using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_DeliverCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid SaleId { get; set; }

		/// <summary>
		/// ชื่อผู้ติดต่อ
		/// </summary>
		public string? ContactName { get; set; }

		/// <summary>
		/// ศูนย์ธุระกิจสินเชื่อมองหมายกิจการสาขาภาค
		/// </summary>
		public int LoanToBranchReg { get; set; }

		/// <summary>
		/// กิจการสาขาภาคมอบหมายผู้จัดการศูนย์สาขา
		/// </summary>
		public int BranchRegToCenBranch { get; set; }

		/// <summary>
		/// ผู้จัดการศูนย์สาขามอบหมายพนักงาน RM
		/// </summary>
		public int CenBranchToRM { get; set; }

		/// <summary>
		/// ปิดการขาย(ครั้ง)
		/// </summary>
		public int CloseSale { get; set; }

		public virtual SaleCustom? Sale { get; set; }
	}
}
