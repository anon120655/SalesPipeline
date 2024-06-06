using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// สินเชื่อ
/// </summary>
public partial class Loan
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

    /// <summary>
    /// ชื่อสินเชื่อ
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// ประเภทการชำระดอกเบี้ย
    /// </summary>
    public Guid? Master_Pre_Interest_PayTypeId { get; set; }

    public string? Master_Pre_Interest_PayTypeName { get; set; }

    /// <summary>
    /// จำนวนช่วงเวลา
    /// </summary>
    public int? PeriodNumber { get; set; }

    /// <summary>
    /// Risk Premium รายปี
    /// </summary>
    public decimal? RiskPremiumYear { get; set; }

    /// <summary>
    /// เงื่อนไข
    /// </summary>
    public string? Condition { get; set; }

    public virtual ICollection<Loan_AppLoan> Loan_AppLoans { get; set; } = new List<Loan_AppLoan>();

    public virtual ICollection<Loan_BusType> Loan_BusTypes { get; set; } = new List<Loan_BusType>();

    public virtual ICollection<Loan_Period> Loan_Periods { get; set; } = new List<Loan_Period>();

    public virtual Master_Pre_Interest_PayType? Master_Pre_Interest_PayType { get; set; }

    public virtual ICollection<Pre_Factor_Info> Pre_Factor_Infos { get; set; } = new List<Pre_Factor_Info>();
}
