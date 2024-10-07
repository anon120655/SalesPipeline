using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
    public class ConfigCode
    {
        /// <summary>
        /// จำนวนครั้งในการล็อกอินผิดพลาด
        /// </summary>
        public const string LOGIN_FAIL = "LOGIN_FAIL";
		/// <summary>
		/// เปิดปิดแถว Z 0=ปิด
		/// </summary>
		public const string CHANCEPASS_Z = "CHANCEPASS_Z";
		/// <summary>
		/// เปิดปิดแถว Limit Multiplier 0=ปิด
		/// </summary>
		public const string CREDITSCORE_LM_MT = "CREDITSCORE_LM_MT";
	}
}
