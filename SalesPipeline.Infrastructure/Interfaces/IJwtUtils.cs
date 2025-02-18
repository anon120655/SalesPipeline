
using SalesPipeline.Utils.Resources.Authorizes.Users;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IJwtUtils
	{
		public string GenerateJwtToken(UserCustom user, int? days = null, int? minutes = null);
		public int? ValidateJwtToken(string? token);
	}
}
