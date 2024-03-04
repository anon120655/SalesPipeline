using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ข้อมูลจังหวัด
/// </summary>
public partial class InfoProvince
{
    public int ProvinceID { get; set; }

    public string ProvinceCode { get; set; } = null!;

    public string ProvinceName { get; set; } = null!;

    public int? RegionID { get; set; }

    /// <summary>
    /// กิจการสาขาภาค
    /// </summary>
    public Guid? Master_Department_BranchId { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
