using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Result
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public Guid SaleId { get; set; }

    /// <summary>
    /// คะแนนรวม
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// ผลการขอสินเชื่อ
    /// </summary>
    public string? ResultLoan { get; set; }

    /// <summary>
    /// โอกาสขอสินเชื่อผ่าน
    /// </summary>
    public string? ChancePercent { get; set; }

    public string? Cr_Level { get; set; }

    public string? Cr_CreditScore { get; set; }

    public string? Cr_Grade { get; set; }

    public string? Cr_LimitMultiplier { get; set; }

    public string? Cr_RateMultiplier { get; set; }

    public string? Ch_Z { get; set; }

    public string? Ch_CreditScore { get; set; }

    public string? Ch_Prob { get; set; }

    public virtual ICollection<Pre_Result_Item> Pre_Result_Items { get; set; } = new List<Pre_Result_Item>();

    public virtual Sale Sale { get; set; } = null!;
}
