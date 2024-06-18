using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.ConstTypeModel
{
    public class ProcessSaleCodeModel
    {
        /// <summary>
        /// รอติดต่อ
        /// </summary>
        public const string WaitContact = "WaitContact";
        /// <summary>
        /// ติดต่อ
        /// </summary>
        public const string Contact = "Contact";
        /// <summary>
        /// เข้าพบ
        /// </summary>
        public const string Meet = "Meet";
        /// <summary>
        /// ยื่นเอกสาร
        /// </summary>
        public const string Document = "Document";
        /// <summary>
        /// ผลลัพธ์
        /// </summary>
        public const string Result = "Result";
        /// <summary>
        /// ปิดการขาย
        /// </summary>
        public const string CloseSale = "CloseSale";
    }
}
