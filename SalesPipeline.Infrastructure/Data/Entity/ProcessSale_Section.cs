using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class ProcessSale_Section
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public Guid ProcessSaleId { get; set; }

    public int SequenceNo { get; set; }

    public string? Name { get; set; }

    public virtual ProcessSale ProcessSale { get; set; } = null!;

    public virtual ICollection<ProcessSale_Reply_Section> ProcessSale_Reply_Sections { get; set; } = new List<ProcessSale_Reply_Section>();

    public virtual ICollection<ProcessSale_Section_Item> ProcessSale_Section_Items { get; set; } = new List<ProcessSale_Section_Item>();
}
