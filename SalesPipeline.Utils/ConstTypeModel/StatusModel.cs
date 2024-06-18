using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
    public class StatusModel
    {
        /// <summary>
        /// -1=ลบ
        /// </summary>
        public const short Delete = -1;
        /// <summary>
        /// 0=ไม่ใช้งาน
        /// </summary>
        public const short InActive = 0;
        /// <summary>
        /// 1=ใช้งาน
        /// </summary>
        public const short Active = 1;
    }
}
