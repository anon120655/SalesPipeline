using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ValidationModel
{
	public class ValidationError
	{
		public string? Field { get; }

		public string? Message { get; }

		public ValidationError(string? field, string? message)
		{
			Field = field != string.Empty ? field : null;
			Message = message;
		}
	}
}
