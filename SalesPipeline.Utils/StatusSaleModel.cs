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
		/// รอมอบหมาย
		/// </summary>
		public const short WaitAssign = 3;
		/// <summary>
		/// รอการติดต่อ
		/// </summary>
		public const short WaitContact = 4;
		/// <summary>
		/// ติดต่อ
		/// </summary>
		public const short Contact = 5;
		/// <summary>
		/// ติดต่อไม่สำเร็จ
		/// </summary>
		public const short ContactFail = 6;
		/// <summary>
		/// รอการเข้าพบ
		/// </summary>
		public const short WaitMeet = 7;
		/// <summary>
		/// เข้าพบ
		/// </summary>
		public const short Meet = 8;
		/// <summary>
		/// เข้าพบไม่สำเร็จ
		/// </summary>
		public const short MeetFail = 9;
		/// <summary>
		/// ยื่นเอกสาร
		/// </summary>
		public const short SubmitDocument = 10;
		/// <summary>
		/// ยื่นเอกสารไม่สำเร็จ
		/// </summary>
		public const short SubmitDocumentFail = 11;
		/// <summary>
		/// ผลลัพธ์
		/// </summary>
		public const short Results = 12;
		/// <summary>
		/// ไม่ผ่านการพิจารณา
		/// </summary>
		public const short ResultsNotConsidered = 13;
		/// <summary>
		/// ไม่ประสงค์กู้
		/// </summary>
		public const short ResultsNotLoan = 14;
		/// <summary>
		/// ปิดการขาย
		/// </summary>
		public const short CloseSale = 15;
		/// <summary>
		/// ปิดการขายไม่สำเร็จ
		/// </summary>
		public const short CloseSaleFail = 16;
	}

}
