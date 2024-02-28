using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Master_List
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public string? Name { get; set; }

    public string? Path { get; set; }

    public virtual ICollection<ProcessSale_Section_ItemOption> ProcessSale_Section_ItemOptions { get; set; } = new List<ProcessSale_Section_ItemOption>();
}
