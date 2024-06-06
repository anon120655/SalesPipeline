using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Factor_Info
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Pre_FactorId { get; set; }

    public Guid? LoanId { get; set; }

    public string? LoanIName { get; set; }

    /// <summary>
    /// จำนวนงวดชำระต่อปี
    /// </summary>
    public int? InstallmentPayYear { get; set; }

    /// <summary>
    /// มูลค่าสินเชื่อ
    /// </summary>
    public decimal? LoanValue { get; set; }

    /// <summary>
    /// ระยะเวลาสินเชื่อ
    /// </summary>
    public int? LoanPeriod { get; set; }

    /// <summary>
    /// ประเภทผู้ขอสินเชื่อ
    /// </summary>
    public Guid Master_Pre_Applicant_LoanId { get; set; }

    public string? Master_Pre_Applicant_LoanName { get; set; }

    /// <summary>
    /// ประเภทธุรกิจ
    /// </summary>
    public Guid Master_Pre_BusinessTypeId { get; set; }

    public string? Master_Pre_BusinessTypeName { get; set; }

    public virtual Loan? Loan { get; set; }

    public virtual Master_Pre_Applicant_Loan Master_Pre_Applicant_Loan { get; set; } = null!;

    public virtual Master_Pre_BusinessType Master_Pre_BusinessType { get; set; } = null!;

    public virtual Pre_Factor Pre_Factor { get; set; } = null!;
}
