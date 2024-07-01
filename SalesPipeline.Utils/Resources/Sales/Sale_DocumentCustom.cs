using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Sales
{
	public class Sale_DocumentCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public Guid SaleId { get; set; }

        public Guid? SaleReplyId { get; set; }

        /// <summary>
        /// รูปบัตรประชาชน
        /// </summary>
        public string? IDCardIMGPath { get; set; }

		/// <summary>
		/// เลขบัตรประชาชน
		/// </summary>
		public string? IDCardNumber { get; set; }

		/// <summary>
		/// ชื่อภาษาไทย
		/// </summary>
		public string? NameTh { get; set; }

		/// <summary>
		/// ชื่อภาษาอังกฤษ
		/// </summary>
		public string? NameEn { get; set; }

		/// <summary>
		/// วันเกิด
		/// </summary>
		public DateTime? Birthday { get; set; }

		/// <summary>
		/// ศาสนา
		/// </summary>
		public string? Religion { get; set; }

		/// <summary>
		/// บ้านเลขที่
		/// </summary>
		public string? HouseNo { get; set; }

		/// <summary>
		/// หมู่ที่
		/// </summary>
		public string? VillageNo { get; set; }

		/// <summary>
		/// จังหวัด
		/// </summary>
		public int? ProvinceId { get; set; }

		public string? ProvinceName { get; set; }

		/// <summary>
		/// อำเภอ
		/// </summary>
		public int? AmphurId { get; set; }

		public string? AmphurName { get; set; }

		/// <summary>
		/// ไฟล์ทะเบียนนบ้าน
		/// </summary>
		public string? HouseRegistrationPath { get; set; }

		/// <summary>
		/// ไฟล์เอกสารอื่นๆ
		/// </summary>
		public string? OtherDocumentPath { get; set; }

		/// <summary>
		/// ประเภทธุรกิจ
		/// </summary>
		public Guid? Master_BusinessTypeId { get; set; }

		public string? Master_BusinessTypeName { get; set; }

		/// <summary>
		/// ลักษณะการดำเนินธุรกิจ
		/// </summary>
		public string? BusinessOperation { get; set; }

		/// <summary>
		/// วันที่รับขึ้นทะเบียนเป็นลูกค้า
		/// </summary>
		public DateTime? RegistrationDate { get; set; }

		/// <summary>
		/// วันที่เริ่มติดต่อกับธนาคารในการขอกู้ครั้งนี้
		/// </summary>
		public DateTime? DateFirstContactBank { get; set; }

		public Guid? Master_TypeLoanRequestId { get; set; }

		public string? Master_TypeLoanRequesName { get; set; }

		/// <summary>
		/// ระบุ
		/// </summary>
		public string? Master_TypeLoanRequestSpecify { get; set; }

		public Guid? Master_ProductProgramBankId { get; set; }

		public string? Master_ProductProgramBankName { get; set; }

		/// <summary>
		/// วงเงินกู้สำหรับเงินทุนหมุนเวียนในกิจการ
		/// </summary>
		public decimal? LoanLimitBusiness { get; set; }

		/// <summary>
		/// วงเงินกู้สำหรับค่าลงทุน
		/// </summary>
		public decimal? LoanLimitInvestmentCost { get; set; }

		/// <summary>
		/// วงเงินกู้สำหรับวัตถุประสงค์
		/// </summary>
		public decimal? LoanLimitObjectiveOther { get; set; }

		/// <summary>
		/// ระบุ
		/// </summary>
		public string? LoanLimitObjectiveOtherSpecify { get; set; }

		/// <summary>
		/// วงเงินรวม
		/// </summary>
		public decimal? TotaLlimit { get; set; }

		/// <summary>
		/// CEQA รวมวงเงินเทียบเท่าสินเชื่อ เท่ากับ
		/// </summary>
		public decimal? TotaLlimitCEQA { get; set; }

		/// <summary>
		/// ความคิดเห็นพนักงานสินเชื่อ
		/// </summary>
		public string? CommentEmployeeLoan { get; set; }

		/// <summary>
		/// รูปลายเซ็นผู้กู้ยืม
		/// </summary>
		public string? SignaturePath { get; set; }

		/// <summary>
		/// วันที่เซ็นผู้กู้ยืม
		/// </summary>
		public DateTime? SignatureDate { get; set; }

		/// <summary>
		/// รูปลายเซ็นพนักงานสินเชื่อ
		/// </summary>
		public string? SignatureEmployeeLoanPath { get; set; }

		/// <summary>
		/// วันที่เซ็นพนักงานสินเชื่อ
		/// </summary>
		public DateTime? SignatureEmployeeLoanDate { get; set; }

		/// <summary>
		/// รูปลายเซ็นผู้จัดการศูนย์
		/// </summary>
		public string? SignatureMCenterPath { get; set; }

		/// <summary>
		/// วันที่เซ็นผู้จัดการศูนย์
		/// </summary>
		public DateTime? SignatureMCenterDate { get; set; }

		/// <summary>
		/// 1=ยื่นกู้
		/// </summary>
		public int? SubmitType { get; set; }

		/// <summary>
		/// วันที่ยื่นกู้
		/// </summary>
		public DateTime? SubmitDate { get; set; }
	}
}
