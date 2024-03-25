using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// สถานะการขาย
/// </summary>
public partial class Master_StatusSale
{
    public int Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public int SequenceNo { get; set; }

    public int? MainId { get; set; }

    public string? NameMain { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// 1=แสดงใน filter
    /// </summary>
    public short? IsShowFilter { get; set; }

    public virtual ICollection<Sale_Contact_History> Sale_Contact_Histories { get; set; } = new List<Sale_Contact_History>();

    public virtual ICollection<Sale_Status> Sale_Statuses { get; set; } = new List<Sale_Status>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
