using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ข้อมูลอำเภอ
/// </summary>
public partial class InfoAmphur
{
    public int ProvinceID { get; set; }

    public int AmphurID { get; set; }

    public string AmphurCode { get; set; } = null!;

    public string AmphurName { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
