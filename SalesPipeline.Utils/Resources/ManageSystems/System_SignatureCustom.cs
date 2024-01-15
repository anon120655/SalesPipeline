using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.ManageSystems
{
	public class System_SignatureCustom : CommonModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public string? Name { get; set; }

		/// <summary>
		/// Url รูป
		/// </summary>
		public string? ImgUrl { get; set; }

		/// <summary>
		/// Url รูปขนาดย่อ
		/// </summary>
		public string? ImgThumbnailUrl { get; set; }

		//Custom
		public FileModel? Files { get; set; }

		public string? ImgUrlv
		{
			get
			{
				if (CreateDate != DateTime.MinValue)
				{
					return $"{ImgUrl}?v={CreateDate.ToString("yyyyMMddHHmmss")}";
				}
				return ImgUrl;
			}
		}
	}
}
