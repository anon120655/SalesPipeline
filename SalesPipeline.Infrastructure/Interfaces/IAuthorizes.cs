using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.iAuthen;
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
		Task<AuthenticateResponse?> AuthenticateBAAC(AuthenticateRequest model, iAuthenResponse modeliAuth);
		UserAuth? GetById(int id);
		Boolean ExpireToken(string? token);
	}
}
