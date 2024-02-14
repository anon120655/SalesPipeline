using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class Error422Model
	{
		public string? status { get; set; }
		public List<Error>? errors { get; set; }

		public class Error
		{
			public string? field { get; set; }
			public string? message { get; set; }
		}

	}
}
