using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

public partial class Dash_Pie
{
    public Guid Id { get; set; }

    /// <summary>
    /// -1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน
    /// </summary>
    public short Status { get; set; }

    public DateTime CreateDate { get; set; }

    public bool IsUpdate { get; set; }

    public int UserId { get; set; }

    /// <summary>
    /// ClosingSale = การปิดการขาย
    /// ReasonNotLoan = เหตุผลไม่ประสงค์ขอสินเชื่อ
    /// NumCusSizeBusiness = จำนวนลูกค้าตามขนาดธุรกิจ
    /// NumCusTypeBusiness = จำนวนลูกค้าตามประเภทธุรกิจ
    /// NumCusISICCode = จำนวนลูกค้าตาม ISIC Code
    /// NumCusLoanType = จำนวนลูกค้าตามประเภทสินเชื่อ
    /// ValueSizeBusiness = มูลค่าสินเชื่อตามขนาดธุรกิจ
    /// ValueTypeBusiness = มูลค่าสินเชื่อตามประเภทธุรกิจ
    /// ValueISICCode = มูลค่าสินเชื่อตาม ISIC Code
    /// ValueLoanType = มูลค่าสินเชื่อตามประเภทสินเชื่อ
    /// </summary>
    public string? Code { get; set; }

    public string? TitleName { get; set; }

    public string? Name { get; set; }

    public decimal? Value { get; set; }
}
