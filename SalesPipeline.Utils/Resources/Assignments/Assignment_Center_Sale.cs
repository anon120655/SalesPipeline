using SalesPipeline.Utils.Resources.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Assignments
{
	public class Assignment_Center_Sale
	{

		public Guid AssignmentId { get; set; }

		/// <summary>
		/// 1=อยู่ในความรับผิดชอบ 0=ถูกเปลี่ยนผู้รับผิดชอบ
		/// </summary>
		public short IsActive { get; set; }

		public Guid SaleId { get; set; }

		public string? Description { get; set; }

		public virtual SaleCustom? Sale { get; set; }

		//Custom
		public bool IsSelect { get; set; }
	}
}
