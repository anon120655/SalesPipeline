using SalesPipeline.Utils.Resources.Authorizes.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IAuthorizes
	{
		Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
		UserAuth? GetById(int id);
		Boolean ExpireToken(string? token);
	}
}
