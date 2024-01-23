using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Reply
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public string? CreateByName { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    public string? UpdateByName { get; set; }

    public Guid SaleId { get; set; }

    public Guid ProcessSaleId { get; set; }

    public string? ProcessSaleName { get; set; }

    public virtual ProcessSale ProcessSale { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;

    public virtual ICollection<Sale_Reply_Section> Sale_Reply_Sections { get; set; } = new List<Sale_Reply_Section>();
}
