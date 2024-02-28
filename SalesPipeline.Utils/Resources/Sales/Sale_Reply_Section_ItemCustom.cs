using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesPipeline.Utils.Resources.ProcessSales;

namespace SalesPipeline.Utils.Resources.Sales
{
    public class Sale_Reply_Section_ItemCustom
    {
        public Guid Id { get; set; }

        /// <summary>
        /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
        /// </summary>
        public short Status { get; set; }

        public Guid SaleReplySectionId { get; set; }

        public Guid PSaleSectionItemId { get; set; }

        public string? ItemLabel { get; set; }

        public string? ItemType { get; set; }

        public virtual ProcessSale_Section_ItemCustom? PSaleSectionItem { get; set; } = null!;

		public virtual Sale_Reply_SectionCustom? SaleReplySection { get; set; } = null!;

		public virtual List<Sale_Reply_Section_ItemValueCustom>? Sale_Reply_Section_ItemValues { get; set; }

		//Custom
		public int ShowType { get; set; }
	}
}
