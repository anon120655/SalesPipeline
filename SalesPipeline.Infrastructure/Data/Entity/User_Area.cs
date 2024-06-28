using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class User_Area
{
    public int Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int UserId { get; set; }

    public int ProvinceId { get; set; }

    public string? ProvinceName { get; set; }

    public virtual User User { get; set; } = null!;
}
