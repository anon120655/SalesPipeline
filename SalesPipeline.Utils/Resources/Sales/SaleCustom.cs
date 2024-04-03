using NPOI.SS.Formula;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class SaleCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public string? CreateByName { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		public string? UpdateByName { get; set; }

		public Guid CustomerId { get; set; }

		/// <summary>
		/// ชื่อบริษัท
		/// </summary>
		public string? CompanyName { get; set; }

		public int? StatusSaleMainId { get; set; }

		/// <summary>
		/// สถานะการขายหลัก
		/// </summary>
		public string? StatusSaleNameMain { get; set; }

		public int StatusSaleId { get; set; }

		/// <summary>
		/// สถานะการขาย
		/// </summary>
		public string? StatusSaleName { get; set; }

		/// <summary>
		/// รายละเอียดสถานะ
		/// </summary>
		public string? StatusDescription { get; set; }

		/// <summary>
		/// เหตุผลไม่ประสงค์กู้
		/// </summary>
		public Guid? Master_Reason_CloseSaleId { get; set; }

		/// <summary>
		/// วันที่นัดหมาย
		/// </summary>
		public DateTime? DateAppointment { get; set; }

		/// <summary>
		/// จำนวนการกู้
		/// </summary>
		public decimal? LoanAmount { get; set; }

		/// <summary>
		/// เปอร์เซ็นโอกาสกู้ผ่าน
		/// </summary>
		public int? PercentChanceLoanPass { get; set; }

		/// <summary>
		/// กิจการสาขาภาค
		/// </summary>
		public Guid? Master_Department_BranchId { get; set; }

		public string? Master_Department_BranchName { get; set; }

		/// <summary>
		/// จังหวัด
		/// </summary>
		public int? ProvinceId { get; set; }

		public string? ProvinceName { get; set; }

		/// <summary>
		/// สาขา
		/// </summary>
		public int? BranchId { get; set; }

		public string? BranchName { get; set; }

		/// <summary>
		/// ผู้จัดการศูนย์ที่ดูแล
		/// </summary>
		public int? AssCenterUserId { get; set; }

		/// <summary>
		/// ชื่อผู้จัดการศูนย์ที่ดูแล
		/// </summary>
		public string? AssCenterUserName { get; set; }

		/// <summary>
		/// ผู้มอบหมายผู้จัดการศูนย์
		/// </summary>
		public int? AssCenterCreateBy { get; set; }

		/// <summary>
		/// วันที่มอบหมายผู้จัดการศูนย์
		/// </summary>
		public DateTime? AssCenterDate { get; set; }

		/// <summary>
		/// พนักงานที่ได้รับมอบหมาย
		/// </summary>
		public int? AssUserId { get; set; }

		/// <summary>
		/// ชื่อพนักงานที่ได้รับมอบหมาย
		/// </summary>
		public string? AssUserName { get; set; }

		public virtual CustomerCustom? Customer { get; set; }

		public virtual UserCustom? AssCenterUser { get; set; }

		public virtual List<Sale_StatusCustom>? Sale_Statuses { get; set; }

		public virtual List<Sale_Contact_HistoryCustom>? Sale_Contact_Histories { get; set; }
		public virtual List<Sale_DocumentCustom>? Sale_Documents { get; set; }

		//Custom
		public bool IsSelected { get; set; }
	}
}
