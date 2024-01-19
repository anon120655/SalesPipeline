using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class RequiredsAttribute : ValidationAttribute
	{
		public RequiredsAttribute()
		{

		}

		public override bool IsValid(object? value)
		{
			return value != null;
		}
	}
}
