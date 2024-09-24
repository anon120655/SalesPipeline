using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	public static class LoanCalculator
	{
		public static double CalculateMonthlyPayment(double principal, double monthlyInterestRate, int numberOfPayments)
		{
			double ratePow = Math.Pow(1 + monthlyInterestRate, numberOfPayments);
			return principal * (monthlyInterestRate * ratePow) / (ratePow - 1);
		}

		public static double CalculateInterestRate(double principal, int numberOfPayments, double targetPayment)
		{
			double low = 0.0;
			double high = 1.0;
			double mid = 0.0;
			double tolerance = 0.0000001;

			while (high - low > tolerance)
			{
				mid = (low + high) / 2;
				double ratePow = Math.Pow(1 + mid, numberOfPayments);
				double calculatedPayment = principal * (mid * ratePow) / (ratePow - 1);

				if (calculatedPayment > targetPayment)
				{
					high = mid;
				}
				else
				{
					low = mid;
				}
			}

			return mid;
		}

		public static XLookUpModel? XLookupList(double lookupValue, List<XLookUpModel> lookUpModel, int searchMode = -1)
		{
			// ค้นหาค่าที่ระบุ ถ้าไม่พบจะคืนค่าลำดับที่น้อยกว่า

			if (lookUpModel == null || lookUpModel.Count == 0)
			{
				return null;
			}

			if (searchMode == -1)
			{
				// ค้นหาจากหลังไปแรก (default)
				for (int i = lookUpModel.Count - 1; i >= 0; i--)
				{
					var checkValue = lookUpModel[i].CheckValue;
					if (lookupValue >= checkValue)
					{
						return lookUpModel[i];
					}
				}

				// ถ้าไม่พบค่าที่ตรงเงื่อนไขจะคืนค่าแรก
				//return lookUpModel[0];
			}
			else
			{
				// ค้นหาจากแรกไปหลัง
				for (int i = 0; i < lookUpModel.Count; i++)
				{
					var checkValue = lookUpModel[i].CheckValue;
					if (lookupValue >= checkValue)
					{
						return lookUpModel[i];
					}
				}

				// ถ้าไม่พบค่าที่ตรงเงื่อนไขจะคืนค่าสุดท้าย
				return lookUpModel[lookUpModel.Count - 1];
			}

			return null;
		}

	}
}
