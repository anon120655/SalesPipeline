using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class System_Config
{
    public int Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public string? Code { get; set; }

    public string? Group { get; set; }

    public string? Value { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
