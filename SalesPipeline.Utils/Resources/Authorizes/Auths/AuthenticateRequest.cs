using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Auths
{
	public class AuthenticateRequest
	{
		[Required]
		public string? Username { get; set; }

		[Required]
		public string? Password { get; set; }

		public string? IPAddress { get; set; }

        public string? DeviceId { get; set; }

        public string? DeviceVersion { get; set; }

		public string? SystemVersion { get; set; }

		public string? AppVersion { get; set; }

		public string? tokenNoti { get; set; }
	}
}
