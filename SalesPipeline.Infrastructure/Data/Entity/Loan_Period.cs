using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// รายละเอียดงวดสินเชื่อ
/// </summary>
public partial class Loan_Period
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    /// <summary>
    /// FK สินเชื่อ
    /// </summary>
    public Guid LoanId { get; set; }

    /// <summary>
    /// ระยะที่
    /// </summary>
    public int PeriodNo { get; set; }

    /// <summary>
    /// FK ประเภทอัตราดอกเบี้ย
    /// </summary>
    public Guid? Master_Pre_Interest_RateTypeId { get; set; }

    public string? Master_Pre_Interest_RateTypeName { get; set; }

    public string? Master_Pre_Interest_RateTypeCode { get; set; }

    /// <summary>
    /// อัตราดอกเบี้ยตาม master
    /// </summary>
    public decimal? RateValueOriginal { get; set; }

    /// <summary>
    /// 1=เพิ่ม 2=ลบ
    /// </summary>
    public int? SpecialType { get; set; }

    /// <summary>
    /// ค่าเพิ่มลบดอกเบี้ย %
    /// </summary>
    public decimal? SpecialRate { get; set; }

    /// <summary>
    /// อัตราดอกเบี้ย %
    /// </summary>
    public decimal? RateValue { get; set; }

    /// <summary>
    /// เริ่มปีที่
    /// </summary>
    public int? StartYear { get; set; }

    public virtual Loan Loan { get; set; } = null!;

    public virtual Master_Pre_Interest_RateType? Master_Pre_Interest_RateType { get; set; }
}
