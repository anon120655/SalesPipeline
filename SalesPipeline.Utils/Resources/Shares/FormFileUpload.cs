using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class FormFileUpload
	{
		public IFormFile FileData { get; set; } = null!;
		public string? Folder { get; set; }
	}
}
