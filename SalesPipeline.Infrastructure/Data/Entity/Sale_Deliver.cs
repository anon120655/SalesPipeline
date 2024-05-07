using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ระยะเวลาในการส่งมอบ
/// </summary>
public partial class Sale_Deliver
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid SaleId { get; set; }

    /// <summary>
    /// ชื่อผู้ติดต่อ
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// ศูนย์ธุระกิจสินเชื่อมองหมายกิจการสาขาภาค
    /// </summary>
    public int LoanToBranchReg { get; set; }

    /// <summary>
    /// กิจการสาขาภาคมอบหมายผู้จัดการศูนย์สาขา
    /// </summary>
    public int BranchRegToCenBranch { get; set; }

    /// <summary>
    /// ผู้จัดการศูนย์สาขามอบหมายพนักงาน RM
    /// </summary>
    public int CenBranchToRM { get; set; }

    /// <summary>
    /// ปิดการขาย(ครั้ง)
    /// </summary>
    public int CloseSale { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}
