using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class UpdateModel
	{
		public string id { get; set; } = null!;
		public string? value { get; set; }
		public int userid { get; set; } = 0; //Default 0 AnonymousUser ผู้ใช้ที่ไม่ได้ลงทะเบียน

		public string SetParameter(bool? isPage = null)
		{
			string? ParameterAll = String.Empty;
			ParameterAll = $"id={id}";
			ParameterAll += $"&value={value}";
			ParameterAll += $"&userid={userid}";
			return ParameterAll;
		}

	}
}
