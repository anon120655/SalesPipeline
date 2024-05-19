using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ประเภทผู้ขอในระยะที่
/// </summary>
public partial class Loan_Period_AppLoan
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid Loan_PeriodId { get; set; }

    public Guid Master_Pre_Applicant_LoanId { get; set; }

    public string? Master_Pre_Applicant_LoanName { get; set; }

    public virtual Loan_Period Loan_Period { get; set; } = null!;
}
