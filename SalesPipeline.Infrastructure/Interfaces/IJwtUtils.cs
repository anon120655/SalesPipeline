
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IJwtUtils
	{
		//public string GenerateJwtToken(UserCustom user, int? days = null, int? minutes = null);
		//public int? ValidateJwtToken(string? token);

		/// <summary>
		/// สร้าง Access Token และ Refresh Token สำหรับผู้ใช้
		/// </summary>
		public Task<(string? AccessToken, string? RefreshToken)> GenerateJwtToken(UserCustom user, int? days = null, int? minutes = null);

		/// <summary>
		/// ตรวจสอบและคืนค่า UserId จาก Access Token
		/// </summary>
		public int? ValidateJwtToken(string? token);

		/// <summary>
		/// ใช้ Refresh Token เพื่อขอ Access Token ใหม่
		/// </summary>
		public Task<(string? AccessToken, string? RefreshToken)> RefreshJwtToken(string refreshToken);
	}
}
