using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Master_NextAction
{
    public int Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    /// <summary>
    /// ลำดับ
    /// </summary>
    public int SequenceNo { get; set; }

    /// <summary>
    /// 1=ติดต่อ
    /// ,2=เข้าพบ
    /// ,3=ยื่นเอกสาร
    /// ,4=ผลลัพธ์
    /// 
    /// </summary>
    public int Type { get; set; }

    public string? Name { get; set; }

    /// <summary>
    /// 1=ไปขั้นตอนถัดไป
    /// </summary>
    public short? IsNext { get; set; }
}
