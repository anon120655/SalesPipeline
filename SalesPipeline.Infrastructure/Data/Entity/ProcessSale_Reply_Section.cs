using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class ProcessSale_Reply_Section
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public Guid PSaleReplyId { get; set; }

    public Guid PSaleSectionId { get; set; }

    public string? Name { get; set; }

    public virtual ProcessSale_Reply PSaleReply { get; set; } = null!;

    public virtual ProcessSale_Section PSaleSection { get; set; } = null!;

    public virtual ICollection<ProcessSale_Reply_Section_Item> ProcessSale_Reply_Section_Items { get; set; } = new List<ProcessSale_Reply_Section_Item>();
}
