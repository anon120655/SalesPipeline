using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Reply_Section_Item
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

    public virtual ProcessSale_Section_Item PSaleSectionItem { get; set; } = null!;

    public virtual Sale_Reply_Section SaleReplySection { get; set; } = null!;

    public virtual ICollection<Sale_Reply_Section_ItemValue> Sale_Reply_Section_ItemValues { get; set; } = new List<Sale_Reply_Section_ItemValue>();
}
