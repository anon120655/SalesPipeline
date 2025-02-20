using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.ConstTypeModel;
using System.Security.Cryptography;
using SalesPipeline.Infrastructure.Wrapper;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class JwtUtils : IJwtUtils
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public JwtUtils(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;

			if (string.IsNullOrEmpty(_appSet.Secret))
				throw new ExceptionCustom("JWT secret not configured");
		}

		//public string GenerateJwtToken(UserCustom user, int? days = null, int? minutes = null)
		//{
		//	DateTime expirationTime;

		//	if (days.HasValue)
		//	{
		//		expirationTime = DateTime.UtcNow.AddDays(days.Value);
		//	}
		//	else if (minutes.HasValue)
		//	{
		//		expirationTime = DateTime.UtcNow.AddMinutes(minutes.Value);
		//	}
		//	else
		//	{
		//		throw new ArgumentException("Either days or minutes must be provided.");
		//	}

		//	// generate token that is valid for {days} days
		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	var key = Encoding.ASCII.GetBytes(_appSet.Secret!);
		//	var tokenDescriptor = new SecurityTokenDescriptor
		//	{
		//		Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
		//		Expires = expirationTime,
		//		SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		//	};
		//	var token = tokenHandler.CreateToken(tokenDescriptor);
		//	return tokenHandler.WriteToken(token);
		//}
				
		public async Task<(string? AccessToken, string? RefreshToken)> GenerateJwtToken(UserCustom user, int? days = null, int? minutes = null)
		{
			DateTime expirationTime = days.HasValue
				? DateTime.UtcNow.AddDays(days.Value)
				: minutes.HasValue
					? DateTime.UtcNow.AddMinutes(minutes.Value)
					: throw new ArgumentException("Either days or minutes must be provided.");

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSet.Secret!);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
				Expires = expirationTime,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			string accessToken = tokenHandler.WriteToken(token);

			// สร้าง Refresh Token
			string refreshToken = GenerateRefreshToken();
			//_refreshTokens[refreshToken] = (user.Id, DateTime.UtcNow.AddDays(7)); // Refresh Token มีอายุ 7 วัน
			await _repo.Authorizes.CreateRefreshJwtToken(new()
			{
				UserId = user.Id,
				TokenValue = refreshToken,
				ExpiryDate = DateTime.UtcNow.AddDays(7)
			});

			return (accessToken, refreshToken);
		}

		/// <summary>
		/// ใช้ Refresh Token เพื่อขอ Access Token ใหม่
		/// </summary>
		public async Task<(string? AccessToken, string? RefreshToken)> RefreshJwtToken(string refreshToken)
		{
			var refreshJwtTokens = await _repo.Authorizes.GetRefreshJwtToken(refreshToken);
			if(refreshJwtTokens == null ) return (null, null);

			// ตรวจสอบว่า Refresh Token หมดอายุหรือไม่
			if (refreshJwtTokens.ExpiryDate < DateTime.UtcNow)
			{
				await _repo.Authorizes.RemoveRefreshJwtToken(refreshToken);
				return (null, null);
			}

			// ลบ Refresh Token เก่า
			await _repo.Authorizes.RemoveRefreshJwtToken(refreshToken);

			// สร้าง Access Token และ Refresh Token ใหม่
			var newTokens = await GenerateJwtToken(new UserCustom { Id = refreshJwtTokens.UserId }, minutes: 15); // Access Token อายุ 15 นาที
			return (newTokens.AccessToken, newTokens.RefreshToken);
		}
				
		private string GenerateRefreshToken()
		{
			var randomBytes = new byte[64];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomBytes);
			}
			return Convert.ToBase64String(randomBytes);
		}
				
		public int? ValidateJwtToken(string? token)
		{
			if (token == null) return null;

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
					ClockSkew = TimeSpan.Zero
				}, out SecurityToken validatedToken);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

				return userId;
			}
			catch
			{
				return null;
			}
		}

	}
}
