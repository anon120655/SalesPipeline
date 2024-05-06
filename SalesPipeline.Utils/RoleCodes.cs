using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	//ADMIN  =  มองเห็นทั้งประเทศ
	//LOAN  =  มองเห็นทั้งประเทศ
	//BRANCH_REG  =  มองเห็นทุกจังหวัดในภาคที่ตัวเองดูแล
	//CEN_BRANCH  =  มองเห็นเฉพาะเคสที่ตัวเองสร้าง และถูกมอบหมายจาก กิจการสาขาภาค และสาขา ที่ดูแล
	//RM  =  มองเห็นเฉพาะเคสที่ตัวเองสร้างและถูกมอบหมาย และสาขา ที่ดูแล
	public class RoleCodes
	{
		/// <summary>
		/// ผู้ดูแลระบบ (SuperAdmin)
		/// </summary>
		public const string SUPERADMIN = "SUPERADMIN";
		/// <summary>
		/// ผู้ดูแลระบบ (Admin)
		/// </summary>
		public const string ADMIN = "ADMIN";
		/// <summary>
		/// สายงานธุรกิจสินเชื่อ
		/// </summary>
		public const string LOAN = "LOAN";
		/// <summary>
		/// สายงานธุรกิจสินเชื่อ 10-12
		/// </summary>
		public const string LOAN01 = "LOAN01";
		/// <summary>
		/// สายงานธุรกิจสินเชื่อ 4-9
		/// </summary>
		public const string LOAN02 = "LOAN02";
		/// <summary>
		/// สายงานกิจการสาขาภาค
		/// </summary>
		public const string BRANCH_REG = "BRANCH_REG";
		/// <summary>
		/// สายงานกิจการสาขาภาค 10-12
		/// </summary>
		public const string BRANCH_REG_01 = "BRANCH_REG_01";
		/// <summary>
		/// สายงานกิจการสาขาภาค 4-9
		/// </summary>
		public const string BRANCH_REG_02 = "BRANCH_REG_02";
		/// <summary>
		/// ผู้จัดการศูนย์สาขา
		/// </summary>
		public const string CEN_BRANCH = "CEN_BRANCH";
		/// <summary>
		/// RM พนักงานขาย
		/// </summary>
		public const string RM = "RM";
	}
}


