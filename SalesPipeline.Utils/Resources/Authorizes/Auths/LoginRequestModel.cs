using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Auths
{
	public class LoginRequestModel
	{
		[Required]
		public string? Username { get; set; }

		[Required]
		public string? Password { get; set; }

		[Required]
		public bool IsRememberMe { get; set; }

		public string? IPAddress { get; set; }
	}
}
