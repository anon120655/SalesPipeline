using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Cal_Fetu_Stan_Score
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_Cal_Fetu_StanId { get; set; }

    /// <summary>
    /// 1=น้ำหนักของแต่ละปัจจัยรายได้ต่อรายจ่าย
    /// 2=น้ำหนักของแต่ละปัจจัยหลักประกันมูลค่าหนี้
    /// 3=น้ำหนักของแต่ละปัจจัยหนี้สินต่อรายได้อื่นๆ
    /// 4=ปริมาณเงินฝาก
    /// 5=ประเภทหลักประกัน
    /// 6=มูลค่าสินเชื่อ
    /// 7=ประวัติการชำระหนี้
    /// </summary>
    public int Type { get; set; }

    public Guid? Pre_Cal_Fetu_StanItemOptionId { get; set; }

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int SequenceNo { get; set; }

    public string? Name { get; set; }

    /// <summary>
    /// คะแนน
    /// </summary>
    public decimal? Score { get; set; }

    public virtual Pre_Cal_Fetu_Stan Pre_Cal_Fetu_Stan { get; set; } = null!;
}
