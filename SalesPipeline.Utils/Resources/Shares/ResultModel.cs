using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class ResultModel<T>
	{
		public bool Status { get; set; } = true;
		public T? Data { get; set; }
		public string? errorMessage { get; set; }
	}
}
