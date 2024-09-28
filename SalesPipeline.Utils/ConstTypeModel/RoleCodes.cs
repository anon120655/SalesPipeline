using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
	//ADMIN  =  มองเห็นทั้งประเทศ
	//LOAN  =  มองเห็นทั้งประเทศ
	//BRANCH_REG  =  มองเห็นทุกจังหวัดในภาคที่ตัวเองดูแล
	//CEN_BRANCH  =  มองเห็นเฉพาะเคสที่ตัวเองสร้าง และถูกมอบหมายจาก กิจการสาขาภาค และสาขา ที่ดูแล
	//RM  =  มองเห็นเฉพาะเคสที่ตัวเองสร้างและถูกมอบหมาย และสาขา ที่ดูแล

    // *** Update ***
	//ROLE อื่นๆ  =  มองเห็นตามพื้นที่ดูแล เช่น ทั้งประเทศ ,ทั้งจังหวัด 1 2 3...
	//ROLE RM  = มองเห็นเฉพาะเคสที่ตัวเองสร้างและถูกมอบหมาย ภายในจังหวัดตามพื้นที่ดูแล
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
        /// ผู้จัดการศูนย์
        /// </summary>
        public const string CENTER = "CENTER";
		/// <summary>
		/// พนักงานธุรกิจสินเชื่อ RM
		/// </summary>
		public const string RM = "RM";
    }
}


