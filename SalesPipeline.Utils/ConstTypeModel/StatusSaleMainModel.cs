using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
    public class StatusSaleMainModel
    {
        /// <summary>
        /// ติดต่อ
        /// </summary>
        public const short Contact = 1;
        /// <summary>
        /// เข้าพบ
        /// </summary>
        public const short Meet = 2;
        /// <summary>
        /// ยื่นเอกสาร
        /// </summary>
        public const short Document = 3;
        /// <summary>
        /// ผลลัพธ์
        /// </summary>
        public const short Result = 4;
        /// <summary>
        /// ปิดการขาย
        /// </summary>
        public const short CloseSale = 5;
    }
}
