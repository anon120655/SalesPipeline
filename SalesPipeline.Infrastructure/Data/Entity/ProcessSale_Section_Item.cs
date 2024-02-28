using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class ProcessSale_Section_Item
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public Guid PSaleSectionId { get; set; }

    public int SequenceNo { get; set; }

    public string? ItemLabel { get; set; }

    public string? ItemType { get; set; }

    /// <summary>
    /// 0=ไม่จำเป็น ,1=จำเป็น
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// 0=แสดงครึ่ง ,1=แสดงเต็ม
    /// </summary>
    public int ShowType { get; set; }

    public virtual ProcessSale_Section PSaleSection { get; set; } = null!;

    public virtual ICollection<ProcessSale_Section_ItemOption> ProcessSale_Section_ItemOptions { get; set; } = new List<ProcessSale_Section_ItemOption>();

    public virtual ICollection<Sale_Reply_Section_Item> Sale_Reply_Section_Items { get; set; } = new List<Sale_Reply_Section_Item>();
}
