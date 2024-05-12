using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class User_Login_LogCustom
	{
		public int Id { get; set; }

		public DateTime CreateDate { get; set; }

		public int UserId { get; set; }

		public string? FullName { get; set; }

		public string? IPAddress { get; set; }
	}
}
