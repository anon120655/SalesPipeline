using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class ErrorCustom
	{
		public int Status { get; set; } = 400; //StatusCodes.Status400BadRequest;
		public string? Message { get; set; }
	}
}
