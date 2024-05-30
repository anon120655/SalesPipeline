using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ตัวแปรคำนวณ คุณสมบัติมาตรฐาน
/// </summary>
public partial class Pre_Cal_Fetu_Stan
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_CalId { get; set; }

    /// <summary>
    /// คะแนนสูงสุด
    /// </summary>
    public int? HighScore { get; set; }

    public virtual Pre_Cal Pre_Cal { get; set; } = null!;

    public virtual ICollection<Pre_Cal_Fetu_Stan_DropDown> Pre_Cal_Fetu_Stan_DropDowns { get; set; } = new List<Pre_Cal_Fetu_Stan_DropDown>();

    public virtual ICollection<Pre_Cal_Fetu_Stan_Score> Pre_Cal_Fetu_Stan_Scores { get; set; } = new List<Pre_Cal_Fetu_Stan_Score>();
}
