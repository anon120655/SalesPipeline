using Microsoft.Extensions.Options;
using SalesPipeline.Utils.ConstTypeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class MenuItemCustom
	{
		//private readonly AppSettings _appSet;

		//public MenuItemCustom(IOptions<AppSettings> appset)
		//{
		//	_appSet = appset.Value;
		//}

		public int Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int MenuNumber { get; set; }

		public int? ParentNumber { get; set; }

		public string Name { get; set; } = null!;

		public string? NameFCC { get; set; }

		public int Sequence { get; set; }

		public string? Path { get; set; }

		public string? ImageUrl { get; set; }

		public string? _NameSystem(string? systemType)
		{
			if (systemType == SystemTypeModel.BAAC)
			{
				return Name;
			}
			else if (systemType == SystemTypeModel.FCC)
			{
				return NameFCC;
			}
			return Name;
		}

	}
}
