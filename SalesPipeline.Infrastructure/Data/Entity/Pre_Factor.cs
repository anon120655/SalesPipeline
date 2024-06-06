using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Factor
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public Guid SaleId { get; set; }

    public Guid Pre_CalId { get; set; }

    /// <summary>
    /// ชื่อบริษัท
    /// </summary>
    public string? CompanyName { get; set; }

    public virtual Pre_Cal Pre_Cal { get; set; } = null!;

    public virtual ICollection<Pre_Factor_App> Pre_Factor_Apps { get; set; } = new List<Pre_Factor_App>();

    public virtual ICollection<Pre_Factor_Bu> Pre_Factor_Bus { get; set; } = new List<Pre_Factor_Bu>();

    public virtual ICollection<Pre_Factor_Info> Pre_Factor_Infos { get; set; } = new List<Pre_Factor_Info>();

    public virtual ICollection<Pre_Factor_Stan> Pre_Factor_Stans { get; set; } = new List<Pre_Factor_Stan>();

    public virtual ICollection<Pre_Result> Pre_Results { get; set; } = new List<Pre_Result>();

    public virtual Sale Sale { get; set; } = null!;
}
