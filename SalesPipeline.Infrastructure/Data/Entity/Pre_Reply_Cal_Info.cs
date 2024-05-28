using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Reply_Cal_Info
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

    public Guid SaleId { get; set; }

    /// <summary>
    /// FK สินเชื่อ
    /// </summary>
    public Guid LoanId { get; set; }

    public string? LoanName { get; set; }

    /// <summary>
    /// จำนวนงวดชำระต่อปี
    /// </summary>
    public int PeriodNumberYear { get; set; }

    /// <summary>
    /// มูลค่าสินเชื่อ
    /// </summary>
    public decimal? LoanAmount { get; set; }

    /// <summary>
    /// ระยะเวลาสินเชื่อ
    /// </summary>
    public int? LoanDuration { get; set; }

    /// <summary>
    /// ประเภทผู้ขอสินเชื่อ
    /// </summary>
    public Guid? Master_Pre_Applicant_LoanId { get; set; }

    public string? Master_Pre_Applicant_LoanName { get; set; }

    /// <summary>
    /// ประเภทธุรกิจ
    /// </summary>
    public Guid? Master_Pre_BusinessTypeId { get; set; }

    public string? Master_Pre_BusinessTypeName { get; set; }

    public virtual Loan Loan { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
