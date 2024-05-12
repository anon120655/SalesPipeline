using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class ChangePasswordModel
	{
		public int UserId { get; set; }
		[Required]
		public string? OldPassword { get; set; }
		[Required]
		public string? NewPassword { get; set; }
		[Required]
		public string? ConfirmPassword { get; set; }
	}
}
