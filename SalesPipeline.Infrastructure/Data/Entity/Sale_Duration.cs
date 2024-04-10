using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// จำนวนวันการดำเนินการแต่ละขั้นตอน
/// </summary>
public partial class Sale_Duration
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid SaleId { get; set; }

    /// <summary>
    /// วันที่เริ่มติดต่อ
    /// </summary>
    public DateTime? ContactStartDate { get; set; }

    /// <summary>
    /// ชื่อผู้ติดต่อ
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// รอการติดต่อ(วัน)
    /// </summary>
    public int WaitContact { get; set; }

    /// <summary>
    /// ติดต่อ(วัน)
    /// </summary>
    public int Contact { get; set; }

    /// <summary>
    /// รอเข้าพบ(วัน)
    /// </summary>
    public int WaitMeet { get; set; }

    /// <summary>
    /// เข้าพบ(วัน)
    /// </summary>
    public int Meet { get; set; }

    /// <summary>
    /// พิจารณาเอกสาร(วัน)
    /// </summary>
    public int Document { get; set; }

    /// <summary>
    /// ผลลัพธ์(วัน)
    /// </summary>
    public int Result { get; set; }

    /// <summary>
    /// ปิดการขาย(วัน)
    /// </summary>
    public int CloseSale { get; set; }

    /// <summary>
    /// รวม
    /// </summary>
    public int TotalDay { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}
