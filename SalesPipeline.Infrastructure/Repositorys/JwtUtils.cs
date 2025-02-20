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

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class JwtUtils : IJwtUtils
	{
		private readonly AppSettings _appSet; 
		private readonly Dictionary<string, (int UserId, DateTime Expiration)> _refreshTokens = new(); // ใช้ Dictionary จำลองฐานข้อมูล

		public JwtUtils(IOptions<AppSettings> appSet)
		{
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

		/// <summary>
		/// สร้าง Access Token และ Refresh Token
		/// </summary>
		public (string AccessToken, string RefreshToken) GenerateJwtToken(UserCustom user, int? days = null, int? minutes = null)
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
			_refreshTokens[refreshToken] = (user.Id, DateTime.UtcNow.AddDays(7)); // Refresh Token มีอายุ 7 วัน

			return (accessToken, refreshToken);
		}

		/// <summary>
		/// ใช้ Refresh Token เพื่อขอ Access Token ใหม่
		/// </summary>
		public (string? AccessToken, string? RefreshToken) RefreshJwtToken(string refreshToken)
		{
			if (!_refreshTokens.ContainsKey(refreshToken)) return (null, null);

			var (userId, expiration) = _refreshTokens[refreshToken];

			// ตรวจสอบว่า Refresh Token หมดอายุหรือไม่
			if (expiration < DateTime.UtcNow)
			{
				_refreshTokens.Remove(refreshToken);
				return (null, null);
			}

			// ลบ Refresh Token เก่า
			_refreshTokens.Remove(refreshToken);

			// สร้าง Access Token และ Refresh Token ใหม่
			var newTokens = GenerateJwtToken(new UserCustom { Id = userId }, minutes: 15); // Access Token อายุ 15 นาที
			return (newTokens.AccessToken, newTokens.RefreshToken);
		}

		/// <summary>
		/// สร้าง Refresh Token ที่ปลอดภัย
		/// </summary>
		private string GenerateRefreshToken()
		{
			var randomBytes = new byte[64];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomBytes);
			}
			return Convert.ToBase64String(randomBytes);
		}

		/// <summary>
		/// ตรวจสอบและคืนค่า userId จาก JWT Token
		/// </summary>
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
