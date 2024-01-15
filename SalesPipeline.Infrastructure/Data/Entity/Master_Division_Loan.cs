using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ฝ่ายธุรกิจสินเชื่อ
/// </summary>
public partial class Master_Division_Loan
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

    /// <summary>
    /// รหัส
    /// </summary>
    public string? Code { get; set; }

    public string? Name { get; set; }

    /// <summary>
    /// สาขาที่ดูแล
    /// </summary>
    public Guid? Division_BranchsId { get; set; }

    public virtual Master_Division_Branch? Division_Branchs { get; set; }
}
