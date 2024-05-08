using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Master_Reason_CloseSale
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateBy { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Sale_Close_Sale> Sale_Close_Sales { get; set; } = new List<Sale_Close_Sale>();

    public virtual ICollection<Sale_Contact> Sale_Contacts { get; set; } = new List<Sale_Contact>();

    public virtual ICollection<Sale_Status> Sale_Statuses { get; set; } = new List<Sale_Status>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
