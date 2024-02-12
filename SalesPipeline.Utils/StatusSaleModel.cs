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
		/// รอมอบหมาย(ศูนย์สาขา)
		/// </summary>
		public const short WaitAssignCenter = 5;
		/// <summary>
		/// รออนุมัติ(ศูนย์สาขา)
		/// </summary>
		public const short WaitApproveCenter = 6;
		/// <summary>
		/// ไม่อนุมัติ(ศูนย์สาขา)
		/// </summary>
		public const short NotApproveCenter = 7;
		/// <summary>
		/// รอมอบหมาย
		/// </summary>
		public const short WaitAssign = 8;
		/// <summary>
		/// รอการติดต่อ
		/// </summary>
		public const short WaitContact = 9;
		/// <summary>
		/// ติดต่อ
		/// </summary>
		public const short Contact = 10;
		/// <summary>
		/// ติดต่อไม่สำเร็จ
		/// </summary>
		public const short ContactFail = 11;
		/// <summary>
		/// รอการเข้าพบ
		/// </summary>
		public const short WaitMeet = 12;
		/// <summary>
		/// เข้าพบ
		/// </summary>
		public const short Meet = 13;
		/// <summary>
		/// เข้าพบไม่สำเร็จ
		/// </summary>
		public const short MeetFail = 14;
		/// <summary>
		/// ยื่นเอกสาร
		/// </summary>
		public const short SubmitDocument = 15;
		/// <summary>
		/// ยื่นเอกสารไม่สำเร็จ
		/// </summary>
		public const short SubmitDocumentFail = 16;
		/// <summary>
		/// ผลลัพธ์
		/// </summary>
		public const short Results = 17;
		/// <summary>
		/// ไม่ผ่านการพิจารณา
		/// </summary>
		public const short ResultsNotConsidered = 18;
		/// <summary>
		/// ไม่ประสงค์กู้
		/// </summary>
		public const short ResultsNotLoan = 19;
		/// <summary>
		/// ปิดการขาย
		/// </summary>
		public const short CloseSale = 20;
		/// <summary>
		/// ปิดการขายไม่สำเร็จ
		/// </summary>
		public const short CloseSaleFail = 21;
	}

}
