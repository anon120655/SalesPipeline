using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
	public class PreStanScoreType
	{
		/// <summary>
		/// น้ำหนักของแต่ละปัจจัยรายได้ต่อรายจ่าย
		/// </summary>
		public const int WeightIncomeExpenses = 1;
		/// <summary>
		/// น้ำหนักของแต่ละปัจจัยหลักประกันมูลค่าหนี้
		/// </summary>
		public const int WeighCollateraltDebtValue = 2;
		/// <summary>
		/// น้ำหนักของแต่ละปัจจัยหนี้สินต่อรายได้อื่นๆ
		/// </summary>
		public const int WeighLiabilitieOtherIncome = 3;
		/// <summary>
		/// ปริมาณเงินฝาก
		/// </summary>
		public const int CashBank = 4;
		/// <summary>
		/// ประเภทหลักประกัน
		/// </summary>
		public const int CollateralType = 5;
		/// <summary>
		/// มูลค่าหลักประกัน
		/// </summary>
		public const int CollateralValue = 6;
		/// <summary>
		/// ประวัติการชำระหนี้
		/// </summary>
		public const int PaymentHistory = 7;
	}
}
