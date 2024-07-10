using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Master_ISICCode
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

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? GroupMaster_BusinessTypeId { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
