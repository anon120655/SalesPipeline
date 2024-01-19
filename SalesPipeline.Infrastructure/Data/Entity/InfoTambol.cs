using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ข้อมูลตำบล
/// </summary>
public partial class InfoTambol
{
    public int id { get; set; }

    public int ProvinceID { get; set; }

    public int AmphurID { get; set; }

    public int TambolID { get; set; }

    public string TambolCode { get; set; } = null!;

    public string TambolName { get; set; } = null!;
}
