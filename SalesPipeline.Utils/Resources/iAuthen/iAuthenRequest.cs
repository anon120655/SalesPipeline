using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.iAuthen
{
	public class iAuthenRequest
	{
		public string? user { get; set; }
		public string? password { get; set; }
		public string? faceID { get; set; }
		public string? requester_id { get; set; }
		public string? reference_id { get; set; }
		public string? ipaddress { get; set; }
		public int authen_type { get; set; }
	}
}
