using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Authorizes.Users;
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
		Task<AuthenticateResponse?> AuthenticateBAAC(AuthenticateRequest model, iAuthenResponse.ResponseData modeliAuth);
		UserAuth? GetById(int id);
		Task CreateRefreshJwtToken(User_RefreshTokenCustom model);
		Task<User_RefreshTokenCustom?> GetRefreshJwtToken(string refreshToken);
		Task RemoveRefreshJwtToken(string refreshToken);
		Task<RefreshTokenResponse> RefreshJwtToken(string refreshToken);
		Boolean ExpireToken(string? token);
		Task RemoveNotiToken(User_Login_LogCustom model);
	}
}
