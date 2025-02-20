using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
    public class RefreshTokenResponse
    {
		public string? access_token { get; set; }
		public string? refresh_token { get; set; }
	}
}
