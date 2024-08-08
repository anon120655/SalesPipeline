using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Phoenix
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid SaleId { get; set; }

    public string? workflow_id { get; set; }

    public string? app_no { get; set; }

    public string? ana_no { get; set; }

    public string? fin_type { get; set; }

    public string? cif_no { get; set; }

    public string? cif_name { get; set; }

    public string? branch_customer { get; set; }

    public string? branch_user { get; set; }

    public string? approve_level { get; set; }

    public string? status_type { get; set; }

    public string? status_code { get; set; }

    public string? create_by { get; set; }

    public string? created_date { get; set; }

    public string? update_by { get; set; }

    public string? update_date { get; set; }

    public string? approve_by { get; set; }

    public string? approve_date { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}
