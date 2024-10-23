using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Pre_Cal
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

    public string? Name { get; set; }

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

    /// <summary>
    /// 1=แสดงเฉพาะคะแนนรวม
    /// </summary>
    public int? DisplayResultType { get; set; }

    public virtual Master_Pre_Applicant_Loan? Master_Pre_Applicant_Loan { get; set; }

    public virtual Master_Pre_BusinessType? Master_Pre_BusinessType { get; set; }

    public virtual ICollection<Pre_Cal_Fetu_App> Pre_Cal_Fetu_Apps { get; set; } = new List<Pre_Cal_Fetu_App>();

    public virtual ICollection<Pre_Cal_Fetu_Bu> Pre_Cal_Fetu_Bus { get; set; } = new List<Pre_Cal_Fetu_Bu>();

    public virtual ICollection<Pre_Cal_Fetu_Stan> Pre_Cal_Fetu_Stans { get; set; } = new List<Pre_Cal_Fetu_Stan>();

    public virtual ICollection<Pre_Cal_Info> Pre_Cal_Infos { get; set; } = new List<Pre_Cal_Info>();

    public virtual ICollection<Pre_Cal_WeightFactor> Pre_Cal_WeightFactors { get; set; } = new List<Pre_Cal_WeightFactor>();

    public virtual ICollection<Pre_Factor> Pre_Factors { get; set; } = new List<Pre_Factor>();
}
