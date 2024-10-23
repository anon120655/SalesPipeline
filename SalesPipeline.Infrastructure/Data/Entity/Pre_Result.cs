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

    public Guid Pre_FactorId { get; set; }

    /// <summary>
    /// คะแนนรวม
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// ผลการขอสินเชื่อ
    /// </summary>
    public string? ResultLoan { get; set; }

    public double? ChanceNumber { get; set; }

    /// <summary>
    /// โอกาสขอสินเชื่อผ่าน
    /// </summary>
    public string? ChancePercent { get; set; }

    public string? Cr_Level { get; set; }

    public int? Cr_CreditScore { get; set; }

    public string? Cr_Grade { get; set; }

    public string? Cr_LimitMultiplier { get; set; }

    public string? Cr_RateMultiplier { get; set; }

    public string? Ch_Z { get; set; }

    public string? Ch_CreditScore { get; set; }

    public string? Ch_Prob { get; set; }

    /// <summary>
    /// ผ่อนทั้งหมด
    /// </summary>
    public decimal? InstallmentAll { get; set; }

    /// <summary>
    /// รายได้ทั้งหมด
    /// </summary>
    public decimal? IncomeTotal { get; set; }

    /// <summary>
    /// อัตราส่วนผ่อน/รายได้
    /// </summary>
    public decimal? RatioInstallmentIncome { get; set; }

    /// <summary>
    /// 1=มีการกดบันทึก
    /// </summary>
    public short? PresSave { get; set; }

    /// <summary>
    /// 1=แสดงเฉพาะคะแนนรวม
    /// </summary>
    public int? DisplayResultType { get; set; }

    public virtual Pre_Factor Pre_Factor { get; set; } = null!;

    public virtual ICollection<Pre_Result_Item> Pre_Result_Items { get; set; } = new List<Pre_Result_Item>();
}
