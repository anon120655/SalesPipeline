
using System.Text.Json.Serialization;


namespace SalesPipeline.Utils.Resources.Authorizes.Auths
{
	public class UserAuth
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Username { get; set; }

		[JsonIgnore]
		public string? Password { get; set; }
	}
}
