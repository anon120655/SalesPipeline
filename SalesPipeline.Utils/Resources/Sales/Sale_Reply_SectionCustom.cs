using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesPipeline.Utils.Resources.ProcessSales;

namespace SalesPipeline.Utils.Resources.Sales
{
    public class Sale_Reply_SectionCustom
    {
        public Guid Id { get; set; }

        /// <summary>
        /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
        /// </summary>
        public short Status { get; set; }

        public Guid SaleReplyId { get; set; }

        public Guid PSaleSectionId { get; set; }

        public string? Name { get; set; }

        public virtual Sale_ReplyCustom? SaleReply { get; set; } = null!;

        public virtual ProcessSale_SectionCustom? PSaleSection { get; set; } = null!;

        public virtual List<Sale_Reply_Section_ItemCustom>? Sale_Reply_Section_Items { get; set; }

        //Custom
        public bool IsSave { get; set; } = true;
    }
}
