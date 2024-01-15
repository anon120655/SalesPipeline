using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ลูกค้า กรรมการบริษัท
/// </summary>
public partial class Customer_Committee
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public int SequenceNo { get; set; }

    public Guid CustomerId { get; set; }

    public string? Name { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
