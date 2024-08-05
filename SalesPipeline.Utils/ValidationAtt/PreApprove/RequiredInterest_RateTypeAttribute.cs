using SalesPipeline.Utils.Resources.Loans;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ValidationAtt.PreApprove
{
	public class RequiredInterest_RateTypeAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			var list = value as List<Loan_AppLoanCustom>;
			if (list != null)
			{
				return list.Count > 0;
			}
			return false;
		}
	}
}
