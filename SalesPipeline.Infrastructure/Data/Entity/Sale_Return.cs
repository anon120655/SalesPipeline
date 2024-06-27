using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Sale_Return
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public string? CreateByName { get; set; }

    public Guid CustomerId { get; set; }

    /// <summary>
    /// ชื่อบริษัท
    /// </summary>
    public string? CompanyName { get; set; }

    public Guid SaleId { get; set; }

    public int StatusSaleId { get; set; }

    /// <summary>
    /// สถานะการขาย
    /// </summary>
    public string? StatusSaleName { get; set; }

    /// <summary>
    /// รายละเอียดสถานะ
    /// </summary>
    public string? StatusDescription { get; set; }

    /// <summary>
    /// ประเภทกิจการ
    /// </summary>
    public Guid? Master_BusinessTypeId { get; set; }

    public string? Master_BusinessTypeName { get; set; }

    /// <summary>
    /// ประเภทสินเชื่อ
    /// </summary>
    public Guid? Master_LoanTypeId { get; set; }

    public string? Master_LoanTypeName { get; set; }

    /// <summary>
    /// พนักงานที่ได้รับมอบหมาย
    /// </summary>
    public int? AssUserId { get; set; }

    /// <summary>
    /// ชื่อพนักงานที่ได้รับมอบหมาย
    /// </summary>
    public string? AssUserName { get; set; }
}
