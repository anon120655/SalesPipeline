using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class UserOnlineModel
	{
		public string? UserKey { get; set; }
		public int Id { get; set; }
		public string? ImgProThumbnailUrl { get; set; }
		public string? FullName { get; set; }
		public string? Ipaddress { get; set; }
		public DateTime? OnlineDate { get; set; }

		public string? ImgUrl
		{
			get
			{
				if (String.IsNullOrEmpty(ImgProThumbnailUrl))
				{
					return "/image/other/defultperson.png";
				}

				return ImgProThumbnailUrl;
			}
		}

	}
}
