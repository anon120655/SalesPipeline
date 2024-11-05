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

		public string? CIF { get; set; }

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
		/// วันที่เริ่มติดต่อ
		/// </summary>
		public DateTime? ContactStartDate { get; set; }

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
		public double? PercentChanceLoanPass { get; set; }

		/// <summary>
		/// เปอร์เซ็นโอกาสกู้ผ่าน
		/// </summary>
		public string? PercentChanceLoanPassName { get; set; }

		public Guid? Pre_FactorId { get; set; }

		/// <summary>
		/// ชื่อโครงการสินเชื่อ
		/// </summary>
		public string? ProjectLoanName { get; set; }

		/// <summary>
		/// ระยะเวลาสินเชื่อ
		/// </summary>
		public int? LoanPeriod { get; set; }

		/// <summary>
		/// กิจการสาขาภาค
		/// </summary>
		public Guid? Master_Branch_RegionId { get; set; }

		public string? Master_Branch_RegionName { get; set; }

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
		/// ผู้จัดการศูนย์ได้รับมอบหมายแล้ว
		/// </summary>
		public bool? AssCenterAlready { get; set; }

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
		/// ได้รับมอบหมายแล้ว
		/// </summary>
		public bool? AssUserAlready { get; set; }

		/// <summary>
		/// พนักงานที่ได้รับมอบหมาย
		/// </summary>
		public int? AssUserId { get; set; }

		/// <summary>
		/// ชื่อพนักงานที่ได้รับมอบหมาย
		/// </summary>
		public string? AssUserName { get; set; }

		/// <summary>
		/// วันที่มอบหมาย
		/// </summary>
		public DateTime? AssUserDate { get; set; }

		/// <summary>
		/// สร้างโดยกด Re-Purpose
		/// </summary>
		public bool? IsRePurpose { get; set; }

		public virtual CustomerCustom? Customer { get; set; }

		public virtual UserCustom? AssCenterUser { get; set; }

		public virtual List<Sale_StatusCustom>? Sale_Statuses { get; set; }

		public virtual ICollection<Sale_ContactCustom>? Sale_Contacts { get; set; }
		public virtual List<Sale_Contact_HistoryCustom>? Sale_Contact_Histories { get; set; }
		public virtual List<Sale_Contact_InfoCustom>? Sale_Contact_Infos { get; set; }
		public virtual ICollection<Sale_MeetCustom>? Sale_Meets { get; set; }
		public virtual List<Sale_DocumentCustom>? Sale_Documents { get; set; }
		public virtual List<Sale_Document_UploadCustom>? Sale_Document_Files { get; set; }
		public virtual ICollection<Sale_ResultCustom>? Sale_Results { get; set; }
		public virtual List<Sale_PartnerCustom>? Sale_Partners { get; set; }


		//Custom
		public bool IsSelected { get; set; }
        public bool IsShowRePurpose { get; set; }
    }
}
