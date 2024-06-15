using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.PreApprove
{
	public class User_Login_TokenNotiCustom
	{
		public int Id { get; set; }

		public DateTime CreateDate { get; set; }

		public int UserId { get; set; }

        public string? DeviceId { get; set; }

        public string? tokenNoti { get; set; }
	}
}
