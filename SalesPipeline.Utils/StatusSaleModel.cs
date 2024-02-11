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
		/// รอมอบหมาย(ศูนย์สาขา)
		/// </summary>
		public const short WaitAssignCenter = 4;
		/// <summary>
		/// รออนุมัติ(ศูนย์สาขา)
		/// </summary>
		public const short WaitApproveCenter = 5;
		/// <summary>
		/// รอมอบหมาย
		/// </summary>
		public const short WaitAssign = 6;
		/// <summary>
		/// รอการติดต่อ
		/// </summary>
		public const short WaitContact = 7;
		/// <summary>
		/// ติดต่อ
		/// </summary>
		public const short Contact = 8;
		/// <summary>
		/// ติดต่อไม่สำเร็จ
		/// </summary>
		public const short ContactFail = 9;
		/// <summary>
		/// รอการเข้าพบ
		/// </summary>
		public const short WaitMeet = 10;
		/// <summary>
		/// เข้าพบ
		/// </summary>
		public const short Meet = 11;
		/// <summary>
		/// เข้าพบไม่สำเร็จ
		/// </summary>
		public const short MeetFail = 12;
		/// <summary>
		/// ยื่นเอกสาร
		/// </summary>
		public const short SubmitDocument = 13;
		/// <summary>
		/// ยื่นเอกสารไม่สำเร็จ
		/// </summary>
		public const short SubmitDocumentFail = 14;
		/// <summary>
		/// ผลลัพธ์
		/// </summary>
		public const short Results = 15;
		/// <summary>
		/// ไม่ผ่านการพิจารณา
		/// </summary>
		public const short ResultsNotConsidered = 16;
		/// <summary>
		/// ไม่ประสงค์กู้
		/// </summary>
		public const short ResultsNotLoan = 17;
		/// <summary>
		/// ปิดการขาย
		/// </summary>
		public const short CloseSale = 18;
		/// <summary>
		/// ปิดการขายไม่สำเร็จ
		/// </summary>
		public const short CloseSaleFail = 19;
	}

}
