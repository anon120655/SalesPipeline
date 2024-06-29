using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class FileModel
	{
		//**** RestSharp ส่งเป็น IFormFile ตรงๆ ไม่ได้ ต้องส่งเป็น byte แต่ฝั่งรับจะใช้เป็น IFormFile
		//FileByte ใช้ส่งผ่าน AddFils RestSharp
		//FileData ใช้รับฝั่ง API

		public AppSettings? appSet { get; set; }
		public string? Id { get; set; }
		public byte[]? FileByte { get; set; }
		public IFormFile? FileData { get; set; }
		public string? ImgBase64Only { get; set; }
		public string? Folder { get; set; }
		public string? FileName { get; set; }
		public long? FileSize { get; set; }
		public string? MimeType { get; set; }
	}
}
