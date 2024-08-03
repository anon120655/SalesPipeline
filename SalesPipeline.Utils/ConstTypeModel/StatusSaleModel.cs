namespace SalesPipeline.Utils.ConstTypeModel
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
        /// รอตรวจสอบ(สขภ.)
        /// </summary>
        public const short WaitVerifyBranch = 3;
        /// <summary>
        /// ไม่ผ่านการตรวจสอบ(สขภ.)
        /// </summary>
        public const short NotVerifyBranch = 4;
        /// <summary>
        /// รอมอบหมาย(ผจศ.)(จาก ศูนย์ธุรกิจสินเชื่อ)
        /// </summary>
        public const short WaitAssignCenterREG = 5;
        /// <summary>
        /// รอมอบหมาย(ผจศ)
        /// </summary>
        public const short WaitAssignCenter = 6;
        /// <summary>
        /// สาขาภาคส่งคืนศูนย์ธุรกิจสินเชื่อ
        /// </summary>
        public const short BranchReturnLCenter = 7;
		/// <summary>
		/// ผจศ. ส่งคืน สำนักงานใหญ่
		/// </summary>
		public const short MCenterReturnLoan = 8;
        /// <summary>
        /// พนักงาน RM ส่งคืน ผจศ.
        /// </summary>
        public const short RMReturnMCenter = 9;
        /// <summary>
        /// รอมอบหมาย
        /// </summary>
        public const short WaitAssign = 10;
        /// <summary>
        /// รอการติดต่อ
        /// </summary>
        public const short WaitContact = 11;
        /// <summary>
        /// ติดต่อ
        /// </summary>
        public const short Contact = 12;
        /// <summary>
        /// ติดต่อไม่สำเร็จ
        /// </summary>
        public const short ContactFail = 13;
        /// <summary>
        /// รอการเข้าพบ
        /// </summary>
        public const short WaitMeet = 14;
        /// <summary>
        /// เข้าพบ
        /// </summary>
        public const short Meet = 15;
        /// <summary>
        /// เข้าพบไม่สำเร็จ
        /// </summary>
        public const short MeetFail = 16;
        /// <summary>
        /// รอยื่นเอกสาร
        /// </summary>
        public const short WaitSubmitDocument = 17;
        /// <summary>
        /// ยื่นเอกสาร
        /// </summary>
        public const short SubmitDocument = 18;
        /// <summary>
        /// ยื่นเอกสารไม่สำเร็จ
        /// </summary>
        public const short SubmitDocumentFail = 19;
        /// <summary>
        /// รอ ผจศ. อนุมัติคำขอสินเชื่อ
        /// </summary>
        public const short WaitApproveLoanRequest = 20;
        /// <summary>
        /// ผจศ. ไม่อนุมัติคำขอสินเชื่อ
        /// </summary>
        public const short NotApproveLoanRequest = 21;
        /// <summary>
        /// รอวิเคราะห์สินเชื่อ(PHOENIX)
        /// </summary>
        public const short WaitAPIPHOENIX = 22;
        /// <summary>
        /// รอบันทึกเลข(CIF)
        /// </summary>
        public const short WaitCIF = 23;
        /// <summary>
        /// รอบันทึกผลลัพธ์
        /// </summary>
        public const short WaitResults = 24;
        /// <summary>
        /// ผลลัพธ์
        /// </summary>
        public const short Results = 25;
        /// <summary>
        /// ไม่ผ่านการพิจารณา
        /// </summary>
        public const short ResultsNotConsidered = 26;
        /// <summary>
        /// รอปิดการขาย
        /// </summary>
        public const short WaitCloseSale = 27;
        /// <summary>
        /// ไม่ประสงค์กู้
        /// </summary>
        public const short CloseSaleNotLoan = 28;
        /// <summary>
        /// ปิดการขาย
        /// </summary>
        public const short CloseSale = 29;
        /// <summary>
        /// ปิดการขายไม่สำเร็จ
        /// </summary>
        public const short CloseSaleFail = 30;
    }

}
