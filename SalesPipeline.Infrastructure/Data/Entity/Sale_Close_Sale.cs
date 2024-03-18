using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Close_Sale
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

    public Guid SaleId { get; set; }

    /// <summary>
    /// ชื่อผู้ติดต่อ
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// เบอร์ติดต่อ
    /// </summary>
    public string? Tel { get; set; }

    /// <summary>
    /// 1=รับสาย 2=ไม่รับสาย
    /// </summary>
    public int? ResultMeetId { get; set; }

    /// <summary>
    /// 1=ประสงค์กู้ 2=ไม่ประสงค์กู้
    /// </summary>
    public int? DesireLoanId { get; set; }

    /// <summary>
    /// 1=ได้รับสินเชื่อจากสถาบันการเงินอื่น
    /// </summary>
    public int? ReasonId { get; set; }

    /// <summary>
    /// บันทึกเพิ่มเติม
    /// </summary>
    public string? Note { get; set; }

    public virtual Sale Sale { get; set; } = null!;
}
