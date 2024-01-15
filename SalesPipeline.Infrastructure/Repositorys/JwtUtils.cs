using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class JwtUtils : IJwtUtils
	{
		private readonly AppSettings _appSet;

		public JwtUtils(IOptions<AppSettings> appSet)
		{
			_appSet = appSet.Value;

			if (string.IsNullOrEmpty(_appSet.Secret))
				throw new ExceptionCustom("JWT secret not configured");
		}

		public string GenerateJwtToken(UserCustom user,int days)
		{
			// generate token that is valid for {days} days
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSet.Secret!);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
				Expires = DateTime.UtcNow.AddDays(days),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public int? ValidateJwtToken(string? token)
		{
			if (token == null)
				return null;

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSet.Secret!);
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

				// return user id from JWT token if validation successful
				return userId;
			}
			catch
			{
				// return null if validation fails
				return null;
			}
		}
	}
}
