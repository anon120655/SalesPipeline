namespace SalesPipeline.Utils
{
	public class StatusSaleModel
	{
		/// <summary>
		/// ไม่บันทึกสถานะ
		/// </summary>
		public const short NotStatus = 0;
		/// <summary>
		/// รออนุมัติ
		/// </summary>
		public const short WaitApprove = 1;
		/// <summary>
		/// ไม่อนุมัติ
		/// </summary>
		public const short NotApprove = 2;
		/// <summary>
		/// รอตรวจสอบ(สาขาภาค)
		/// </summary>
		public const short WaitVerifyBranch = 3;
		/// <summary>
		/// ไม่ผ่านการตรวจสอบ(สาขาภาค)
		/// </summary>
		public const short NotVerifyBranch = 4;
		/// <summary>
		/// รอมอบหมาย(ผจศ)
		/// </summary>
		public const short WaitAssignCenter = 5;
		/// <summary>
		/// สาขาภาคส่งคืนศูนย์ธุรกิจสินเชื่อ
		/// </summary>
		public const short BranchReturnLCenter = 6;
		/// <summary>
		/// ผจศ. ส่งคืน สาขาภาค
		/// </summary>
		public const short MCenterReturnBranch = 7;
		/// <summary>
		/// พนักงาน RM ส่งคืน ผจศ.
		/// </summary>
		public const short RMReturnMCenter = 8;

		/// <summary>
		/// รอมอบหมาย
		/// </summary>
		public const short WaitAssign = 9;
		/// <summary>
		/// รอการติดต่อ
		/// </summary>
		public const short WaitContact = 10;
		/// <summary>
		/// ติดต่อ
		/// </summary>
		public const short Contact = 11;
		/// <summary>
		/// ติดต่อไม่สำเร็จ
		/// </summary>
		public const short ContactFail = 12;
		/// <summary>
		/// รอการเข้าพบ
		/// </summary>
		public const short WaitMeet = 13;
		/// <summary>
		/// เข้าพบ
		/// </summary>
		public const short Meet = 14;
		/// <summary>
		/// เข้าพบไม่สำเร็จ
		/// </summary>
		public const short MeetFail = 15;
		/// <summary>
		/// รอยื่นเอกสาร
		/// </summary>
		public const short WaitSubmitDocument = 16;
		/// <summary>
		/// ยื่นเอกสาร
		/// </summary>
		public const short SubmitDocument = 17;
		/// <summary>
		/// ยื่นเอกสารไม่สำเร็จ
		/// </summary>
		public const short SubmitDocumentFail = 18;
		/// <summary>
		/// รอ ผจศ. อนุมัติเอกสาร
		/// </summary>
		public const short WaitApproveDocument = 19;
		/// <summary>
		/// ผจศ. ไม่อนุมัติเอกสาร
		/// </summary>
		public const short NotApproveDocument = 20;
		/// <summary>
		/// รอวิเคราะห์สินเชื่อ(LPS)
		/// </summary>
		public const short WaitAPIPHOENIXLPS = 21;
		/// <summary>
		/// รอผลพิจารณา
		/// </summary>
		public const short WaitResults = 22;
		/// <summary>
		/// ผลลัพธ์
		/// </summary>
		public const short Results = 23;
		/// <summary>
		/// ไม่ผ่านการพิจารณา
		/// </summary>
		public const short ResultsNotConsidered = 24;
		/// <summary>
		/// ไม่ประสงค์กู้
		/// </summary>
		public const short ResultsNotLoan = 25;
		/// <summary>
		/// รอปิดการขาย
		/// </summary>
		public const short WaitCloseSale = 26;
		/// <summary>
		/// ปิดการขาย
		/// </summary>
		public const short CloseSale = 27;
		/// <summary>
		/// ปิดการขายไม่สำเร็จ
		/// </summary>
		public const short CloseSaleFail = 28;
	}

}
