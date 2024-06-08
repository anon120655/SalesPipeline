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
			var result = principal * (monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, numberOfPayments)) /
				   (Math.Pow(1 + monthlyInterestRate, numberOfPayments) - 1);

			return result;
		}
	}
}
