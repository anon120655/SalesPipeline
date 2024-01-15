using SalesPipeline.Utils.Resources.Authorizes.Users;
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
        public string? expires_in { get; set; }

        public AuthenticateResponse(UserCustom user, string token, string expires)
		{
			Id = user.Id;
			FirstName = user.FirstName;
			LastName = user.LastName;
			Email = user.Email;
			access_token = token;
			expires_in = expires;
		}
	}
}
