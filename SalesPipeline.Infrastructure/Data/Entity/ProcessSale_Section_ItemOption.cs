using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class ProcessSale_Section_ItemOption
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public Guid PSaleSectionItemId { get; set; }

    public int SequenceNo { get; set; }

    public string? OptionLabel { get; set; }

    public string? DefaultValue { get; set; }

    public virtual ProcessSale_Section_Item PSaleSectionItem { get; set; } = null!;

    public virtual ICollection<ProcessSale_Reply_Section_ItemValue> ProcessSale_Reply_Section_ItemValues { get; set; } = new List<ProcessSale_Reply_Section_ItemValue>();
}
