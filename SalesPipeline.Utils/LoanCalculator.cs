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
			//var result = principal * (monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, numberOfPayments)) /
			//	   (Math.Pow(1 + monthlyInterestRate, numberOfPayments) - 1);

			//return result;
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
	}
}
