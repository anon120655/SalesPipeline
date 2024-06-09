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

		public static string XLookup(double lookupValue, double[] lookupArray, string[] returnArray, string ifNotFound)
		{
			if (lookupArray.Length != returnArray.Length)
			{
				throw new ArgumentException("Lookup array and return array must be of the same length.");
			}

			// ค้นหาค่าที่ระบุ ถ้าไม่พบจะคืนค่าลำดับที่น้อยกว่า
			for (int i = lookupArray.Length - 1; i >= 0; i--)
			{
				if (lookupValue >= lookupArray[i])
				{
					return returnArray[i];
				}
			}

			// ถ้าไม่พบค่าที่ตรงกันหรือน้อยกว่าค่าที่กำหนด, คืนค่าที่กำหนดไว้ (ifNotFound)
			return ifNotFound;
		}

	}
}
