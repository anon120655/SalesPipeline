using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.iAuthen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Auths
{
	public class AuthenticateResponse
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? access_token { get; set; }
		public string? refresh_token { get; set; }
		public string? expires_in { get; set; }
        public iAuthenResponse? iauthen { get; set; }

        public AuthenticateResponse(UserCustom user, string? token, string expires, string? refresh)
		{
			Id = user.Id;
			FirstName = user.FirstName;
			LastName = user.LastName;
			Email = user.Email;
			access_token = token;
			refresh_token = refresh;
			expires_in = expires;
		}
	}
}
