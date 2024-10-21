
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.ValidationAtt;

namespace SalesPipeline.Utils.Resources.Customers
{
	public class CustomerCustom : CommonModel
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

		/// <summary>
		/// Role แรกที่เพิ่มข้อมูล
		/// </summary>
		public string? InsertRoleCode { get; set; }

		/// <summary>
		/// วันที่เข้ามาติดต่อ
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public DateTime? DateContact { get; set; }

		/// <summary>
		/// ช่องทางการติดต่อ
		/// </summary>
		public Guid? Master_ContactChannelId { get; set; }

		public string? Master_ContactChannelName { get; set; }

		/// <summary>
		/// จังหวัดผู้ติดต่อ
		/// </summary>
		public int? ContactProvinceId { get; set; }

		public string? ContactProvinceName { get; set; }

		/// <summary>
		/// สาขา
		/// </summary>
		public int? BranchId { get; set; }

		/// <summary>
		/// ชื่อสาขา
		/// </summary>
		public string? BranchName { get; set; }

		/// <summary>
		/// สำนักงานจังหวัด (สนจ.)
		/// </summary>
		public string? ProvincialOffice { get; set; }

		/// <summary>
		/// ชื่อพนักงาน
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? EmployeeName { get; set; }

		/// <summary>
		/// รหัสพนักงาน
		/// </summary>
		public string? EmployeeId { get; set; }

		/// <summary>
		/// ชื่อผู้ติดต่อ
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? ContactName { get; set; }

		/// <summary>
		/// โทรศัพท์
		/// </summary>
		public string? ContactTel { get; set; }

		public string? CIF { get; set; }

		/// <summary>
		/// ชื่อบริษัท
		/// </summary>
		[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? CompanyName { get; set; }

		/// <summary>
		/// เลขทะเบียนนิติบุคคล
		/// </summary>
		//[Required(ErrorMessage = "กรุณาระบุข้อมูล")]
		//[MinLength(13, ErrorMessage = "ระบุข้อมูล 13 หลัก")]
		public string? JuristicPersonRegNumber { get; set; }

		/// <summary>
		/// ประเภทกิจการ
		/// </summary>
		public Guid? Master_BusinessTypeId { get; set; }

		public string? Master_BusinessTypeName { get; set; }

		/// <summary>
		/// ขนาดธุรกิจ
		/// </summary>
		public Guid? Master_BusinessSizeId { get; set; }

		public string? Master_BusinessSizeName { get; set; }

		/// <summary>
		/// ISIC Code
		/// </summary>
		public Guid? Master_ISICCodeId { get; set; }

		public string? Master_ISICCodeName { get; set; }

		/// <summary>
		/// ประเภทธุรกิจ (TSIC)
		/// </summary>
		public Guid? Master_TSICId { get; set; }

		public string? Master_TSICName { get; set; }

		/// <summary>
		/// ผลผลิตหลัก
		/// </summary>
		public Guid? Master_YieldId { get; set; }

		public string? Master_YieldName { get; set; }

		/// <summary>
		/// ห่วงโซ่คุณค่า 
		/// </summary>
		public Guid? Master_ChainId { get; set; }

		public string? Master_ChainName { get; set; }

		/// <summary>
		/// ประเภทสินเชื่อ
		/// </summary>
		public Guid? Master_LoanTypeId { get; set; }

		public string? Master_LoanTypeName { get; set; }

		/// <summary>
		/// อีเมลบริษัท
		/// </summary>
		public string? CompanyEmail { get; set; }

		/// <summary>
		/// โทรศัพท์บริษัท
		/// </summary>
		public string? CompanyTel { get; set; }

		/// <summary>
		/// กลุ่มบริษัทแม่
		/// </summary>
		public string? ParentCompanyGroup { get; set; }

		/// <summary>
		/// ถนน/ซอย/หมู่บ้าน
		/// </summary>
		public string? Road_Soi_Village { get; set; }

		/// <summary>
		/// บ้านเลขที่
		/// </summary>
		[CustomerAddressAtt]
		public string? HouseNo { get; set; }

		/// <summary>
		/// หมู่ที่
		/// </summary>
		public int? VillageNo { get; set; }

		/// <summary>
		/// จังหวัด
		/// </summary>
		[CustomerAddressAtt]
		public int? ProvinceId { get; set; }

		public string? ProvinceName { get; set; }

		/// <summary>
		/// อำเภอ
		/// </summary>
		[CustomerAddressAtt]
		public int? AmphurId { get; set; }

		public string? AmphurName { get; set; }

		/// <summary>
		/// ตำบล
		/// </summary>
		[CustomerAddressAtt]
		public int? TambolId { get; set; }

		public string? TambolName { get; set; }

		/// <summary>
		/// รหัสไปรษณีย์
		/// </summary>
		//[CustomerAtt(ErrorMessage = "กรุณาระบุข้อมูล")]
		public string? ZipCode { get; set; }

		/// <summary>
		/// วันประชุมผู้ถือหุ้น
		/// </summary>
		public DateTime? ShareholderMeetDay { get; set; }

		/// <summary>
		/// ทุนจดทะเบียน
		/// </summary>
		public string? RegisteredCapital { get; set; }

		/// <summary>
		/// Credit Score
		/// </summary>
		public string? CreditScore { get; set; }

		/// <summary>
		/// ปีงบการเงิน
		/// </summary>
		public string? FiscalYear { get; set; }

		/// <summary>
		/// วันเดือนปีงบการเงิน
		/// </summary>
		public DateTime? StatementDate { get; set; }

		/// <summary>
		/// ลูกหนี้การค้า
		/// </summary>
		public decimal? TradeAccReceivable { get; set; }

		/// <summary>
		/// ลูกหนี้การค้าและตั่วเงินรับ-สุทธิ
		/// </summary>
		public decimal? TradeAccRecProceedsNet { get; set; }

		/// <summary>
		/// สินค้าคงเหลือ
		/// </summary>
		public decimal? Inventories { get; set; }

		/// <summary>
		/// เงินให้กู้ยืมระยะสั้น
		/// </summary>
		public decimal? LoansShort { get; set; }

		/// <summary>
		/// รวมสินทรัพย์หมุนเวียน
		/// </summary>
		public decimal? TotalCurrentAssets { get; set; }

		/// <summary>
		/// เงินให้กู้ยืมระยะยาว
		/// </summary>
		public decimal? LoansLong { get; set; }

		/// <summary>
		/// ที่ดิน อาคาร และอุปกรณ์
		/// </summary>
		public decimal? LandBuildingEquipment { get; set; }

		/// <summary>
		/// รวมสินทรัพย์ไม่หมุนเวียน
		/// </summary>
		public decimal? TotalNotCurrentAssets { get; set; }

		/// <summary>
		/// รวมสินทรัพย์
		/// </summary>
		public decimal? AssetsTotal { get; set; }

		/// <summary>
		/// เจ้าหนี้การค้า
		/// </summary>
		public decimal? TradeAccPay { get; set; }

		/// <summary>
		/// เงินกู้ระยะสั้น
		/// </summary>
		public decimal? TradeAccPayLoansShot { get; set; }

		/// <summary>
		/// รวมหนี้สินหมุนเวียน
		/// </summary>
		public decimal? TradeAccPayTotalCurrentLia { get; set; }

		/// <summary>
		/// เงินกู้ระยะยาว
		/// </summary>
		public decimal? TradeAccPayLoansLong { get; set; }

		/// <summary>
		/// รวมหนี้สินไม่หมุนเวียน
		/// </summary>
		public decimal? TradeAccPayTotalNotCurrentLia { get; set; }

		/// <summary>
		/// เงินให้กู้ยืมระยะสั้น
		/// </summary>
		public decimal? TradeAccPayForLoansShot { get; set; }

		/// <summary>
		/// รวมหนี้สิน
		/// </summary>
		public decimal? TradeAccPayTotalLiabilitie { get; set; }

		/// <summary>
		/// ทุนจดทะเบียนสามัญ
		/// </summary>
		public decimal? RegisterCapitalOrdinary { get; set; }

		/// <summary>
		/// ทุนจดทะเบียนที่ชำระแล้ว
		/// </summary>
		public decimal? RegisterCapitalPaid { get; set; }

		/// <summary>
		/// กำไร (ขาดทุน) สะสม
		/// </summary>
		public decimal? ProfitLossAccumulate { get; set; }

		/// <summary>
		/// รวมส่วนของผู้ถือหุ้น
		/// </summary>
		public decimal? TotalShareholders { get; set; }

		/// <summary>
		/// รวมหนี้สินและส่วนของผู้ถือหุ้น
		/// </summary>
		public decimal? TotalLiabilitieShareholders { get; set; }

		/// <summary>
		/// รายได้รวม
		/// </summary>
		public decimal? TotalIncome { get; set; }

		/// <summary>
		/// ต้นทุนขาย
		/// </summary>
		public decimal? CostSales { get; set; }

		/// <summary>
		/// กำไรขั้นต้น
		/// </summary>
		public decimal? GrossProfit { get; set; }

		/// <summary>
		/// ค่าใช้จ่ายในการดำเนินงาน
		/// </summary>
		public decimal? OperatingExpenses { get; set; }

		/// <summary>
		/// กำไร (ขาดทุน) ก่อนหักค่าเสื่อมและค่าใช้จ่าย
		/// </summary>
		public decimal? ProfitLossBeforeDepExp { get; set; }

		/// <summary>
		/// กำไร (ขาดทุน) ก่อนหักดอกเบี้ยและภาษี
		/// </summary>
		public decimal? ProfitLossBeforeInterestTax { get; set; }

		/// <summary>
		/// กำไร(ขาดทุน) ก่อนหักดอกเบี้ยและภาษีเงินได้
		/// </summary>
		public decimal? ProfitLossBeforeIncomeTaxExpense { get; set; }

		/// <summary>
		/// กำไร (ขาดทุน) สุทธิ
		/// </summary>
		public decimal? NetProfitLoss { get; set; }

		/// <summary>
		/// สินเชื่อที่สนใจ
		/// </summary>
		public string? InterestLoan { get; set; }

		/// <summary>
		/// ระบุ
		/// </summary>
		public string? InterestLoanSpecify { get; set; }

		/// <summary>
		/// จุดประสงค์การกู้
		/// </summary>
		public string? InterestObjectiveLoan { get; set; }

		/// <summary>
		/// วงเงิน
		/// </summary>
		public decimal? InterestCreditLimit { get; set; }

		/// <summary>
		/// หมายเหตุ
		/// </summary>
		public string? InterestNote { get; set; }

		public virtual List<Customer_CommitteeCustom>? Customer_Committees { get; set; }

		public virtual List<Customer_ShareholderCustom>? Customer_Shareholders { get; set; }

		public virtual List<SaleCustom>? Sales { get; set; }

		//Custom
		public int? StatusSaleId { get; set; }
		//ใช้เช็คกรณีสร้างโดย สายงานธุรกิจสินเชื่อ หรือ Admin เพื่อส่งให้ กิจการสาขาภาคนั้นๆ
		public Guid? Branch_RegionId { get; set; }
		public string? Branch_RegionName { get; set; }
		public bool IsSelectVersion { get; set; } = false;
		public bool IsKeep { get; set; } = false;
		public bool? IsValidate { get; set; }
		public List<string?>? ValidateError { get; set; }
		public bool? IsRePurpose { get; set; }
		//ข้ามการตรวจสอบที่อยู่
		public bool? IsExceptValidAddress { get; set; }
	}
}
