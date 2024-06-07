using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Factor_Stan
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_FactorId { get; set; }

    /// <summary>
    /// รายได้
    /// </summary>
    public decimal? Income { get; set; }

    /// <summary>
    /// รายจ่าย
    /// </summary>
    public decimal? Expenses { get; set; }

    /// <summary>
    /// หนี้สินอื่นๆ
    /// </summary>
    public decimal? OtherDebts { get; set; }

    /// <summary>
    /// รายได้ที่ได้ตามระยะงวดหนี้สินด้านบน
    /// </summary>
    public decimal? IncomeDebtPeriod { get; set; }

    /// <summary>
    /// ปริมาณเงินฝากกับ ธกส.
    /// </summary>
    public decimal? DepositBAAC { get; set; }

    /// <summary>
    /// ประเภทหลักประกัน
    /// </summary>
    public Guid? Stan_ItemOptionId_Type1 { get; set; }

    public string? Stan_ItemOptionName_Type1 { get; set; }

    /// <summary>
    /// มูลค่าหลักประกัน
    /// </summary>
    public decimal? Stan_ItemOptionValue_Type1 { get; set; }

    /// <summary>
    /// ประวัติการชำระหนี้
    /// </summary>
    public Guid? Stan_ItemOptionId_Type2 { get; set; }

    public string? Stan_ItemOptionName_Type2 { get; set; }

    public virtual Pre_Factor Pre_Factor { get; set; } = null!;
}
