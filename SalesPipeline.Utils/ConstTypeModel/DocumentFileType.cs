using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
	public class DocumentFileType
	{
		/// <summary>
		/// บัตรประชาชน
		/// </summary>
		public const int IDCard = 1;
		/// <summary>
		/// ทะเบียนนบ้าน
		/// </summary>
		public const int HouseRegistration = 2;
		/// <summary>
		/// เอกสารอื่นๆ
		/// </summary>
		public const int Other = 3;
		/// <summary>
		/// ลายเซ็นผู้กู้ยืม
		/// </summary>
		public const int Customer = 4;
		/// <summary>
		/// ลายเซ็นพนักงานสินเชื่อ
		/// </summary>
		public const int RM = 5;
		/// <summary>
		/// เอกสารเพิ่มเติม
		/// </summary>
		public const int More = 6;
	}
}
