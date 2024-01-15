using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Entity;

/// <summary>
/// ลูกค้า
/// </summary>
public partial class Customer
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
    /// วันที่เข้ามาติดต่อ
    /// </summary>
    public DateTime? DateContact { get; set; }

    /// <summary>
    /// ช่องทางการติดต่อ
    /// </summary>
    public int? ContactChannelId { get; set; }

    /// <summary>
    /// สาขา
    /// </summary>
    public int? BranchId { get; set; }

    /// <summary>
    /// สำนักงานจังหวัด (สนจ.)
    /// </summary>
    public string? ProvincialOffice { get; set; }

    /// <summary>
    /// ชื่อพนักงาน
    /// </summary>
    public string? EmployeeName { get; set; }

    /// <summary>
    /// รหัสพนักงาน
    /// </summary>
    public int? EmployeeId { get; set; }

    /// <summary>
    /// ชื่อผู้ติดต่อ
    /// </summary>
    public string? ContactName { get; set; }

    /// <summary>
    /// โทรศัพท์
    /// </summary>
    public string? ContactTel { get; set; }

    /// <summary>
    /// ชื่อบริษัท
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// เลขทะเบียนนิติบุคคล
    /// </summary>
    public string? JuristicPersonRegNumber { get; set; }

    /// <summary>
    /// ประเภทกิจการ
    /// </summary>
    public string? BusinessType { get; set; }

    /// <summary>
    /// ขนาดธุรกิจ
    /// </summary>
    public string? BusinessSize { get; set; }

    /// <summary>
    /// ผลผลิตหลัก
    /// </summary>
    public string? MainProduction { get; set; }

    /// <summary>
    /// ห่วงโซ่คุณค่า 
    /// </summary>
    public string? ValueChain { get; set; }

    /// <summary>
    /// อีเมลบริษัท
    /// </summary>
    public string? CompanyEmail { get; set; }

    /// <summary>
    /// โทรศัพท์บริษัท
    /// </summary>
    public string? CompanyTel { get; set; }

    /// <summary>
    /// กลุ่มบริษัทแม่
    /// </summary>
    public string? ParentCompanyGroup { get; set; }

    /// <summary>
    /// บ้านเลขที่
    /// </summary>
    public string? HouseNo { get; set; }

    /// <summary>
    /// หมู่ที่
    /// </summary>
    public int? VillageNo { get; set; }

    /// <summary>
    /// จังหวัด
    /// </summary>
    public int? ProvinceId { get; set; }

    /// <summary>
    /// อำเภอ
    /// </summary>
    public int? AmphurId { get; set; }

    /// <summary>
    /// ตำบล
    /// </summary>
    public int? TambolId { get; set; }

    /// <summary>
    /// รหัสไปรษณีย์
    /// </summary>
    public string? ZipCode { get; set; }

    /// <summary>
    /// วันประชุมผู้ถือหุ้น
    /// </summary>
    public DateTime? ShareholderMeetDay { get; set; }

    /// <summary>
    /// ทุนจดทะเบียน
    /// </summary>
    public string? RegisteredCapital { get; set; }

    public string? CreditScore { get; set; }

    /// <summary>
    /// ปีงบประมาณ
    /// </summary>
    public string? FiscalYear { get; set; }

    /// <summary>
    /// วันเดือนปีงบการเงิน
    /// </summary>
    public DateTime? StatementDate { get; set; }

    /// <summary>
    /// ลูกหนี้การค้า
    /// </summary>
    public string? TradeAccReceivable { get; set; }

    /// <summary>
    /// ลูกหนี้การค้าและตั่วเงินรับ-สุทธิ
    /// </summary>
    public string? TradeAccRecProceedsNet { get; set; }

    /// <summary>
    /// สินค้าคงเหลือ
    /// </summary>
    public string? Inventories { get; set; }

    /// <summary>
    /// สัญชาติ
    /// </summary>
    public string? Nationality { get; set; }

    /// <summary>
    /// สัดส่วนการถือหุ้น
    /// </summary>
    public string? Proportion { get; set; }

    /// <summary>
    /// จำนวนหุ้นที่ถือ
    /// </summary>
    public int? NumberShareholder { get; set; }

    /// <summary>
    /// ชื่อผู้ถือหุ้น
    /// </summary>
    public string? NameShareholder { get; set; }

    public virtual ICollection<Customer_Committee> Customer_Committees { get; set; } = new List<Customer_Committee>();

    public virtual ICollection<Customer_Shareholder> Customer_Shareholders { get; set; } = new List<Customer_Shareholder>();
}
