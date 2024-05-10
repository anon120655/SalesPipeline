using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
	public class User_Target_SaleCustom
	{
		public Guid Id { get; set; }

		/// <summary>
		/// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
		/// </summary>
		public short Status { get; set; }

		public DateTime CreateDate { get; set; }

		public int CreateBy { get; set; }

		public DateTime UpdateDate { get; set; }

		public int UpdateBy { get; set; }

		/// <summary>
		/// พนักงาน
		/// </summary>
		public int UserId { get; set; }

		public int Year { get; set; }

		/// <summary>
		/// ยอดเป้าหมาย 
		/// </summary>
		public decimal AmountTarget { get; set; }

		/// <summary>
		/// ยอดที่ทำได้
		/// </summary>
		public decimal AmountActual { get; set; }

		public virtual UserCustom? User { get; set; }


		//Custom
		public string? AmountTargetStr
		{
			get
			{
				return AmountTarget.ToString(GeneralTxt.FormatDecimal);
			}
		}

		public string? AmountActualStr
		{
			get
			{
				return AmountActual.ToString(GeneralTxt.FormatDecimal);
			}
		}

		public bool IsSuccessTarger
		{
			get
			{
				return AmountActual >= AmountTarget;
			}
		}

	}
}
