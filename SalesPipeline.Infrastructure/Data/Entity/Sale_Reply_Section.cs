using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Reply_Section
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public Guid SaleReplyId { get; set; }

    public Guid PSaleSectionId { get; set; }

    public string? Name { get; set; }

    public virtual ProcessSale_Section PSaleSection { get; set; } = null!;

    public virtual Sale_Reply SaleReply { get; set; } = null!;

    public virtual ICollection<Sale_Reply_Section_Item> Sale_Reply_Section_Items { get; set; } = new List<Sale_Reply_Section_Item>();
}
