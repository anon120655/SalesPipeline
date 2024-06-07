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
		/// อัตราส่วนรายได้ต่อรายจ่าย
		/// </summary>
		public const int WeightIncomeExpenses = 1;
		/// <summary>
		/// อัตราส่วนหลักประกันต่อมูลค่าหนี้
		/// </summary>
		public const int WeighCollateraltDebtValue = 2;
		/// <summary>
		/// อัตราส่วนหนี้สินอื่นๆต่อรายได้
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
