using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Reply_Section_ItemValue
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public Guid SaleReplySectionItemId { get; set; }

    public Guid PSaleSectionItemOptionId { get; set; }

    public string? OptionLabel { get; set; }

    public string? ReplyValue { get; set; }

    public string? ReplyName { get; set; }

    public DateTime? ReplyDate { get; set; }

    public TimeOnly? ReplyTime { get; set; }

    public Guid? FileId { get; set; }

    public string? FileUrl { get; set; }

    public string? FileName { get; set; }

    public Guid? Master_ListId { get; set; }

    public virtual FileUpload? File { get; set; }

    public virtual ProcessSale_Section_ItemOption PSaleSectionItemOption { get; set; } = null!;

    public virtual Sale_Reply_Section_Item SaleReplySectionItem { get; set; } = null!;
}
