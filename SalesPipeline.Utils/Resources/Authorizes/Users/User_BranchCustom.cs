using SalesPipeline.Utils.Resources.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Authorizes.Users
{
    public class User_BranchCustom
    {
        public int Id { get; set; }

        /// <summary>
        /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
        /// </summary>
        public short Status { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public int UserId { get; set; }

        public int BranchId { get; set; }

        public virtual UserCustom? User { get; set; }
    }
}
