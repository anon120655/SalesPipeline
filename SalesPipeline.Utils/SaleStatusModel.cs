namespace SalesPipeline.Utils
{
	public class SaleStatusModel
	{
		/// <summary>
		/// รอการติดต่อ
		/// </summary>
		public const short WaitContact = 1;
		/// <summary>
		/// ติดต่อ
		/// </summary>
		public const short Contact = 2;
		/// <summary>
		/// รอการเข้าพบ
		/// </summary>
		public const short WaitMeet = 3;
		/// <summary>
		/// เข้าพบ
		/// </summary>
		public const short Meet = 4;
		/// <summary>
		/// ยื่นเอกสาร
		/// </summary>
		public const short SubmitDocuments = 5;
		/// <summary>
		/// ผลลัพธ์
		/// </summary>
		public const short Results = 6;
		/// <summary>
		/// ปิดการขาย
		/// </summary>
		public const short CloseSale = 7;
		/// <summary>
		/// ปิดการขายไม่สำเร็จ
		/// </summary>
		public const short CloseSaleFail = 8;
	}

}
