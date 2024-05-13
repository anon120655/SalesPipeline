using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Infrastructure.Data.Entity;

namespace SalesPipeline.Infrastructure.Data.Context;

public partial class SalesPipelineContext : DbContext
{
    public SalesPipelineContext(DbContextOptions<SalesPipelineContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment_BranchReg> Assignment_BranchRegs { get; set; }

    public virtual DbSet<Assignment_CenterBranch> Assignment_CenterBranches { get; set; }

    public virtual DbSet<Assignment_RM> Assignment_RMs { get; set; }

    public virtual DbSet<Assignment_RM_Sale> Assignment_RM_Sales { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customer_Committee> Customer_Committees { get; set; }

    public virtual DbSet<Customer_Shareholder> Customer_Shareholders { get; set; }

    public virtual DbSet<Dash_Avg_Number> Dash_Avg_Numbers { get; set; }

    public virtual DbSet<Dash_Map_Thailand> Dash_Map_Thailands { get; set; }

    public virtual DbSet<Dash_Pie> Dash_Pies { get; set; }

    public virtual DbSet<Dash_Status_Total> Dash_Status_Totals { get; set; }

    public virtual DbSet<FileUpload> FileUploads { get; set; }

    public virtual DbSet<InfoAmphur> InfoAmphurs { get; set; }

    public virtual DbSet<InfoBranch> InfoBranches { get; set; }

    public virtual DbSet<InfoProvince> InfoProvinces { get; set; }

    public virtual DbSet<InfoTambol> InfoTambols { get; set; }

    public virtual DbSet<Logging> Loggings { get; set; }

    public virtual DbSet<Master_Branch_Region> Master_Branch_Regions { get; set; }

    public virtual DbSet<Master_BusinessSize> Master_BusinessSizes { get; set; }

    public virtual DbSet<Master_BusinessType> Master_BusinessTypes { get; set; }

    public virtual DbSet<Master_Chain> Master_Chains { get; set; }

    public virtual DbSet<Master_ContactChannel> Master_ContactChannels { get; set; }

    public virtual DbSet<Master_Department> Master_Departments { get; set; }

    public virtual DbSet<Master_ISICCode> Master_ISICCodes { get; set; }

    public virtual DbSet<Master_List> Master_Lists { get; set; }

    public virtual DbSet<Master_LoanType> Master_LoanTypes { get; set; }

    public virtual DbSet<Master_Meet> Master_Meets { get; set; }

    public virtual DbSet<Master_NextAction> Master_NextActions { get; set; }

    public virtual DbSet<Master_Position> Master_Positions { get; set; }

    public virtual DbSet<Master_Proceed> Master_Proceeds { get; set; }

    public virtual DbSet<Master_ProductProgramBank> Master_ProductProgramBanks { get; set; }

    public virtual DbSet<Master_ReasonReturn> Master_ReasonReturns { get; set; }

    public virtual DbSet<Master_Reason_CloseSale> Master_Reason_CloseSales { get; set; }

    public virtual DbSet<Master_SLAOperation> Master_SLAOperations { get; set; }

    public virtual DbSet<Master_StatusSale> Master_StatusSales { get; set; }

    public virtual DbSet<Master_TSIC> Master_TSICs { get; set; }

    public virtual DbSet<Master_TypeLoanRequest> Master_TypeLoanRequests { get; set; }

    public virtual DbSet<Master_Year> Master_Years { get; set; }

    public virtual DbSet<Master_Yield> Master_Yields { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<ProcessSale> ProcessSales { get; set; }

    public virtual DbSet<ProcessSale_Section> ProcessSale_Sections { get; set; }

    public virtual DbSet<ProcessSale_Section_Item> ProcessSale_Section_Items { get; set; }

    public virtual DbSet<ProcessSale_Section_ItemOption> ProcessSale_Section_ItemOptions { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Sale_Activity> Sale_Activities { get; set; }

    public virtual DbSet<Sale_Close_Sale> Sale_Close_Sales { get; set; }

    public virtual DbSet<Sale_Contact> Sale_Contacts { get; set; }

    public virtual DbSet<Sale_Contact_History> Sale_Contact_Histories { get; set; }

    public virtual DbSet<Sale_Contact_Info> Sale_Contact_Infos { get; set; }

    public virtual DbSet<Sale_Deliver> Sale_Delivers { get; set; }

    public virtual DbSet<Sale_Document> Sale_Documents { get; set; }

    public virtual DbSet<Sale_Duration> Sale_Durations { get; set; }

    public virtual DbSet<Sale_Meet> Sale_Meets { get; set; }

    public virtual DbSet<Sale_Partner> Sale_Partners { get; set; }

    public virtual DbSet<Sale_Reply> Sale_Replies { get; set; }

    public virtual DbSet<Sale_Reply_Section> Sale_Reply_Sections { get; set; }

    public virtual DbSet<Sale_Reply_Section_Item> Sale_Reply_Section_Items { get; set; }

    public virtual DbSet<Sale_Reply_Section_ItemValue> Sale_Reply_Section_ItemValues { get; set; }

    public virtual DbSet<Sale_Result> Sale_Results { get; set; }

    public virtual DbSet<Sale_Return> Sale_Returns { get; set; }

    public virtual DbSet<Sale_Status> Sale_Statuses { get; set; }

    public virtual DbSet<Sale_Status_Total> Sale_Status_Totals { get; set; }

    public virtual DbSet<SendMail_Log> SendMail_Logs { get; set; }

    public virtual DbSet<SendMail_Template> SendMail_Templates { get; set; }

    public virtual DbSet<System_SLA> System_SLAs { get; set; }

    public virtual DbSet<System_Signature> System_Signatures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<User_Level> User_Levels { get; set; }

    public virtual DbSet<User_Login_Log> User_Login_Logs { get; set; }

    public virtual DbSet<User_Permission> User_Permissions { get; set; }

    public virtual DbSet<User_Role> User_Roles { get; set; }

    public virtual DbSet<User_Target_Sale> User_Target_Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Assignment_BranchReg>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Assignment_BranchReg");

            entity.HasIndex(e => e.BranchId, "BranchId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.BranchCode)
                .HasMaxLength(255)
                .HasComment("รหัสสาขา");
            entity.Property(e => e.BranchId).HasColumnType("int(11)");
            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .HasComment("ชื่อสาขา");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CurrentNumber)
                .HasComment("จำนวนลูกค้าปัจจุบันที่ดูแล")
                .HasColumnType("int(11)");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasComment("รหัสพนักงาน");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .HasComment("ชื่อพนักงาน");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel).HasMaxLength(255);
            entity.Property(e => e.UserId)
                .HasComment("UserId กิจการสาขาภาค")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Branch).WithMany(p => p.Assignment_BranchRegs)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("assignment_branchreg_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Assignment_BranchRegs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_branchreg_ibfk_1");
        });

        modelBuilder.Entity<Assignment_CenterBranch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Assignment_CenterBranch");

            entity.HasIndex(e => e.BranchId, "Master_Department_BranchId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.BranchCode)
                .HasMaxLength(255)
                .HasComment("รหัสสาขา");
            entity.Property(e => e.BranchId).HasColumnType("int(11)");
            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .HasComment("สาขา");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CurrentNumber)
                .HasComment("จำนวนลูกค้าปัจจุบันที่ดูแล")
                .HasColumnType("int(11)");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasComment("รหัสพนักงานผู้จัดการศูนย์");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้จัดการศูนย์");
            entity.Property(e => e.RMNumber)
                .HasComment("จำนวนพนง.สินเชื่อ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel).HasMaxLength(255);
            entity.Property(e => e.UserId)
                .HasComment("UserId ผู้จัดการศูนย์ที่ได้รับมอบหมาย")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Branch).WithMany(p => p.Assignment_CenterBranches)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("assignment_centerbranch_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Assignment_CenterBranches)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_centerbranch_ibfk_1");
        });

        modelBuilder.Entity<Assignment_RM>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Assignment_RM", tb => tb.HasComment("พนักงานที่ถูกมอบหมาย"));

            entity.HasIndex(e => e.BranchId, "BranchId");

            entity.HasIndex(e => e.Status, "Status");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.BranchId)
                .HasComment("สาขา")
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CurrentNumber)
                .HasComment("จำนวนลูกค้าปัจจุบันที่ดูแล")
                .HasColumnType("int(11)");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasComment("รหัสพนักงาน");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .HasComment("ชื่อพนักงานที่ได้รับมอบหมาย");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UserId)
                .HasComment("พนักงานที่ได้รับมอบหมาย")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Branch).WithMany(p => p.Assignment_RMs)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("assignment_rm_ibfk_3");

            entity.HasOne(d => d.User).WithMany(p => p.Assignment_RMs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_rm_ibfk_2");
        });

        modelBuilder.Entity<Assignment_RM_Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Assignment_RM_Sale", tb => tb.HasComment("ลูกค้าที่พนักงานดูแล"));

            entity.HasIndex(e => e.AssignmentRMId, "AssignmentId");

            entity.HasIndex(e => e.CreateBy, "CreateBy");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CreateBy)
                .HasComment("ผู้มอบหมาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateByName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้มอบหมาย");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsActive)
                .HasComment("1=อยู่ในความรับผิดชอบ 0=ถูกเปลี่ยนผู้รับผิดชอบ")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.SaleId).HasComment("สินเชื่อที่ถูกมอบหมาย");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.AssignmentRM).WithMany(p => p.Assignment_RM_Sales)
                .HasForeignKey(d => d.AssignmentRMId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_rm_sale_ibfk_1");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Assignment_RM_Sales)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_rm_sale_ibfk_3");

            entity.HasOne(d => d.Sale).WithMany(p => p.Assignment_RM_Sales)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_rm_sale_ibfk_2");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Customer", tb => tb.HasComment("ลูกค้า"));

            entity.HasIndex(e => e.AmphurId, "AmphurId");

            entity.HasIndex(e => e.CreateBy, "CreateBy");

            entity.HasIndex(e => e.Master_BusinessSizeId, "Master_BusinessSizeId");

            entity.HasIndex(e => e.Master_BusinessTypeId, "Master_BusinessTypeId");

            entity.HasIndex(e => e.Master_ChainId, "Master_ChainId");

            entity.HasIndex(e => e.Master_ContactChannelId, "Master_ContactChannelId");

            entity.HasIndex(e => e.Master_ISICCodeId, "Master_ISICCodeId");

            entity.HasIndex(e => e.Master_LoanTypeId, "Master_LoanTypeId");

            entity.HasIndex(e => e.Master_TSICId, "Master_TSICId");

            entity.HasIndex(e => e.Master_YieldId, "Master_YieldId");

            entity.HasIndex(e => e.ProvinceId, "ProvinceId");

            entity.HasIndex(e => e.TambolId, "TambolId");

            entity.Property(e => e.AmphurId)
                .HasComment("อำเภอ")
                .HasColumnType("int(11)");
            entity.Property(e => e.AmphurName).HasMaxLength(255);
            entity.Property(e => e.AssetsTotal)
                .HasPrecision(18, 2)
                .HasComment("รวมสินทรัพย์");
            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .HasComment("สาขา");
            entity.Property(e => e.CIF).HasMaxLength(255);
            entity.Property(e => e.CompanyEmail)
                .HasMaxLength(255)
                .HasComment("อีเมลบริษัท");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasComment("ชื่อบริษัท");
            entity.Property(e => e.CompanyTel)
                .HasMaxLength(255)
                .HasComment("โทรศัพท์บริษัท");
            entity.Property(e => e.ContactName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.ContactTel)
                .HasMaxLength(255)
                .HasComment("โทรศัพท์");
            entity.Property(e => e.CostSales)
                .HasPrecision(18, 2)
                .HasComment("ต้นทุนขาย");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreditScore)
                .HasMaxLength(255)
                .HasComment("Credit Score");
            entity.Property(e => e.DateContact)
                .HasComment("วันที่เข้ามาติดต่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasComment("รหัสพนักงาน");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .HasComment("ชื่อพนักงาน");
            entity.Property(e => e.FiscalYear)
                .HasMaxLength(255)
                .HasComment("ปีงบการเงิน");
            entity.Property(e => e.GrossProfit)
                .HasPrecision(18, 2)
                .HasComment("กำไรขั้นต้น");
            entity.Property(e => e.HouseNo)
                .HasMaxLength(255)
                .HasComment("บ้านเลขที่");
            entity.Property(e => e.InsertRoleCode)
                .HasMaxLength(50)
                .HasComment("Role แรกที่เพิ่มข้อมูล");
            entity.Property(e => e.InterestCreditLimit)
                .HasPrecision(18, 2)
                .HasComment("วงเงิน");
            entity.Property(e => e.InterestLoan)
                .HasMaxLength(255)
                .HasComment("สินเชื่อที่สนใจ");
            entity.Property(e => e.InterestLoanSpecify)
                .HasMaxLength(255)
                .HasComment("ระบุ");
            entity.Property(e => e.InterestNote)
                .HasMaxLength(1000)
                .HasComment("หมายเหตุ");
            entity.Property(e => e.InterestObjectiveLoan)
                .HasMaxLength(255)
                .HasComment("จุดประสงค์การกู้");
            entity.Property(e => e.Inventories)
                .HasPrecision(18, 2)
                .HasComment("สินค้าคงเหลือ");
            entity.Property(e => e.JuristicPersonRegNumber)
                .HasMaxLength(255)
                .HasComment("เลขทะเบียนนิติบุคคล");
            entity.Property(e => e.LandBuildingEquipment)
                .HasPrecision(18, 2)
                .HasComment("ที่ดิน อาคาร และอุปกรณ์");
            entity.Property(e => e.LoansLong)
                .HasPrecision(18, 2)
                .HasComment("เงินให้กู้ยืมระยะยาว");
            entity.Property(e => e.LoansShort)
                .HasPrecision(18, 2)
                .HasComment("เงินให้กู้ยืมระยะสั้น");
            entity.Property(e => e.Master_BusinessSizeId).HasComment("ขนาดธุรกิจ");
            entity.Property(e => e.Master_BusinessSizeName).HasMaxLength(255);
            entity.Property(e => e.Master_BusinessTypeId).HasComment("ประเภทกิจการ");
            entity.Property(e => e.Master_BusinessTypeName).HasMaxLength(255);
            entity.Property(e => e.Master_ChainId).HasComment("ห่วงโซ่คุณค่า ");
            entity.Property(e => e.Master_ChainName).HasMaxLength(255);
            entity.Property(e => e.Master_ContactChannelId).HasComment("ช่องทางการติดต่อ");
            entity.Property(e => e.Master_ContactChannelName).HasMaxLength(255);
            entity.Property(e => e.Master_ISICCodeId).HasComment("ISIC Code");
            entity.Property(e => e.Master_ISICCodeName).HasMaxLength(255);
            entity.Property(e => e.Master_LoanTypeId).HasComment("ประเภทสินเชื่อ");
            entity.Property(e => e.Master_LoanTypeName).HasMaxLength(255);
            entity.Property(e => e.Master_TSICId).HasComment("ประเภทธุรกิจ (TSIC)");
            entity.Property(e => e.Master_TSICName).HasMaxLength(255);
            entity.Property(e => e.Master_YieldId).HasComment("ผลผลิตหลัก");
            entity.Property(e => e.Master_YieldName).HasMaxLength(255);
            entity.Property(e => e.NetProfitLoss)
                .HasPrecision(18, 2)
                .HasComment("กำไร (ขาดทุน) สุทธิ");
            entity.Property(e => e.OperatingExpenses)
                .HasPrecision(18, 2)
                .HasComment("ค่าใช้จ่ายในการดำเนินงาน");
            entity.Property(e => e.ParentCompanyGroup)
                .HasMaxLength(255)
                .HasComment("กลุ่มบริษัทแม่");
            entity.Property(e => e.ProfitLossAccumulate)
                .HasPrecision(18, 2)
                .HasComment("กำไร (ขาดทุน) สะสม");
            entity.Property(e => e.ProfitLossBeforeDepExp)
                .HasPrecision(18, 2)
                .HasComment("กำไร (ขาดทุน) ก่อนหักค่าเสื่อมและค่าใช้จ่าย");
            entity.Property(e => e.ProfitLossBeforeIncomeTaxExpense)
                .HasPrecision(18, 2)
                .HasComment("กำไร(ขาดทุน) ก่อนหักดอกเบี้ยและภาษีเงินได้");
            entity.Property(e => e.ProfitLossBeforeInterestTax)
                .HasPrecision(18, 2)
                .HasComment("กำไร (ขาดทุน) ก่อนหักดอกเบี้ยและภาษี");
            entity.Property(e => e.ProvinceId)
                .HasComment("จังหวัด")
                .HasColumnType("int(11)");
            entity.Property(e => e.ProvinceName).HasMaxLength(255);
            entity.Property(e => e.ProvincialOffice)
                .HasMaxLength(255)
                .HasComment("สำนักงานจังหวัด (สนจ.)");
            entity.Property(e => e.RegisterCapitalOrdinary)
                .HasPrecision(18, 2)
                .HasComment("ทุนจดทะเบียนสามัญ");
            entity.Property(e => e.RegisterCapitalPaid)
                .HasPrecision(18, 2)
                .HasComment("ทุนจดทะเบียนที่ชำระแล้ว");
            entity.Property(e => e.RegisteredCapital)
                .HasMaxLength(255)
                .HasComment("ทุนจดทะเบียน");
            entity.Property(e => e.Road_Soi_Village)
                .HasMaxLength(255)
                .HasComment("ถนน/ซอย/หมู่บ้าน");
            entity.Property(e => e.ShareholderMeetDay)
                .HasComment("วันประชุมผู้ถือหุ้น")
                .HasColumnType("datetime");
            entity.Property(e => e.StatementDate)
                .HasComment("วันเดือนปีงบการเงิน")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.TambolId)
                .HasComment("ตำบล")
                .HasColumnType("int(11)");
            entity.Property(e => e.TambolName).HasMaxLength(255);
            entity.Property(e => e.TotalCurrentAssets)
                .HasPrecision(18, 2)
                .HasComment("รวมสินทรัพย์หมุนเวียน");
            entity.Property(e => e.TotalIncome)
                .HasPrecision(18, 2)
                .HasComment("รายได้รวม");
            entity.Property(e => e.TotalLiabilitieShareholders)
                .HasPrecision(18, 2)
                .HasComment("รวมหนี้สินและส่วนของผู้ถือหุ้น");
            entity.Property(e => e.TotalNotCurrentAssets)
                .HasPrecision(18, 2)
                .HasComment("รวมสินทรัพย์ไม่หมุนเวียน");
            entity.Property(e => e.TotalShareholders)
                .HasPrecision(18, 2)
                .HasComment("รวมส่วนของผู้ถือหุ้น");
            entity.Property(e => e.TradeAccPay)
                .HasPrecision(18, 2)
                .HasComment("เจ้าหนี้การค้า");
            entity.Property(e => e.TradeAccPayForLoansShot)
                .HasPrecision(18, 2)
                .HasComment("เงินให้กู้ยืมระยะสั้น");
            entity.Property(e => e.TradeAccPayLoansLong)
                .HasPrecision(18, 2)
                .HasComment("เงินกู้ระยะยาว");
            entity.Property(e => e.TradeAccPayLoansShot)
                .HasPrecision(18, 2)
                .HasComment("เงินกู้ระยะสั้น");
            entity.Property(e => e.TradeAccPayTotalCurrentLia)
                .HasPrecision(18, 2)
                .HasComment("รวมหนี้สินหมุนเวียน");
            entity.Property(e => e.TradeAccPayTotalLiabilitie)
                .HasPrecision(18, 2)
                .HasComment("รวมหนี้สิน");
            entity.Property(e => e.TradeAccPayTotalNotCurrentLia)
                .HasPrecision(18, 2)
                .HasComment("รวมหนี้สินไม่หมุนเวียน");
            entity.Property(e => e.TradeAccRecProceedsNet)
                .HasPrecision(18, 2)
                .HasComment("ลูกหนี้การค้าและตั่วเงินรับ-สุทธิ");
            entity.Property(e => e.TradeAccReceivable)
                .HasPrecision(18, 2)
                .HasComment("ลูกหนี้การค้า");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.VillageNo)
                .HasComment("หมู่ที่")
                .HasColumnType("int(11)");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(255)
                .HasComment("รหัสไปรษณีย์");

            entity.HasOne(d => d.Amphur).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AmphurId)
                .HasConstraintName("customer_ibfk_9");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_ibfk_1");

            entity.HasOne(d => d.Master_BusinessSize).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Master_BusinessSizeId)
                .HasConstraintName("customer_ibfk_4");

            entity.HasOne(d => d.Master_BusinessType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Master_BusinessTypeId)
                .HasConstraintName("customer_ibfk_3");

            entity.HasOne(d => d.Master_Chain).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Master_ChainId)
                .HasConstraintName("customer_ibfk_7");

            entity.HasOne(d => d.Master_ContactChannel).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Master_ContactChannelId)
                .HasConstraintName("customer_ibfk_2");

            entity.HasOne(d => d.Master_ISICCode).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Master_ISICCodeId)
                .HasConstraintName("customer_ibfk_5");

            entity.HasOne(d => d.Master_LoanType).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Master_LoanTypeId)
                .HasConstraintName("customer_ibfk_11");

            entity.HasOne(d => d.Master_TSIC).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Master_TSICId)
                .HasConstraintName("customer_ibfk_12");

            entity.HasOne(d => d.Master_Yield).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Master_YieldId)
                .HasConstraintName("customer_ibfk_6");

            entity.HasOne(d => d.Province).WithMany(p => p.Customers)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("customer_ibfk_8");

            entity.HasOne(d => d.Tambol).WithMany(p => p.Customers)
                .HasForeignKey(d => d.TambolId)
                .HasConstraintName("customer_ibfk_10");
        });

        modelBuilder.Entity<Customer_Committee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Customer_Committee", tb => tb.HasComment("ลูกค้า กรรมการบริษัท"));

            entity.HasIndex(e => e.CustomerId, "CustomerId");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Customer_Committees)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_committee_ibfk_1");
        });

        modelBuilder.Entity<Customer_Shareholder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Customer_Shareholder", tb => tb.HasComment("ลูกค้า ผู้ถือหุ้น"));

            entity.HasIndex(e => e.CustomerId, "CustomerId");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ถือหุ้น");
            entity.Property(e => e.Nationality)
                .HasMaxLength(255)
                .HasComment("สัญชาติ");
            entity.Property(e => e.NumberShareholder)
                .HasComment("จำนวนหุ้นที่ถือ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Proportion)
                .HasMaxLength(255)
                .HasComment("สัดส่วนการถือหุ้น");
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.TotalShareValue)
                .HasPrecision(18, 2)
                .HasComment("มูลค่าหุ้นทั้งหมด");

            entity.HasOne(d => d.Customer).WithMany(p => p.Customer_Shareholders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_shareholder_ibfk_1");
        });

        modelBuilder.Entity<Dash_Avg_Number>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Dash_Avg_Number");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.AvgDealOrg)
                .HasComment("ดีลโดยเฉลี่ยต่อองค์กร")
                .HasColumnType("int(11)");
            entity.Property(e => e.AvgDealRM)
                .HasComment("ดีลโดยเฉลี่ยต่อพนักงานสินเชื่อ")
                .HasColumnType("int(11)");
            entity.Property(e => e.AvgDeliveryTime)
                .HasComment("ระยะเวลาเฉลี่ยในการส่งมอบ")
                .HasColumnType("int(11)");
            entity.Property(e => e.AvgPerDeal)
                .HasPrecision(18, 2)
                .HasComment("มูลค่าเฉลี่ยต่อหนึ่งดีล");
            entity.Property(e => e.AvgSaleActcloseDeal)
                .HasComment("กิจกรรมการขายโดยเฉลี่ยต่อดีลที่ปิดการขาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.AvgTimeCloseSale)
                .HasComment("ระยะเวลาเฉลี่ยที่ใช้ในการปิดการขาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.AvgTimeLostSale)
                .HasComment("ระยะเวลาเฉลี่ยที่ใช้ในการขายที่แพ้ให้กับคู่แข่ง")
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.Dash_Avg_Numbers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dash_avg_number_ibfk_1");
        });

        modelBuilder.Entity<Dash_Map_Thailand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Dash_Map_Thailand");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ProvinceId).HasColumnType("int(11)");
            entity.Property(e => e.ProvinceName).HasMaxLength(255);
            entity.Property(e => e.SalesAmount)
                .HasPrecision(18, 2)
                .HasComment("ยอดขาย");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Type)
                .HasComment("1=ยอดขายสูงสุด 2=แพ้ให้กับคู่แข่งสูงสุด")
                .HasColumnType("int(11)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.Dash_Map_Thailands)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dash_map_thailand_ibfk_1");
        });

        modelBuilder.Entity<Dash_Pie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Dash_Pie");

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasComment("ClosingSale = การปิดการขาย\r\nReasonNotLoan = เหตุผลไม่ประสงค์ขอสินเชื่อ\r\nNumCusSizeBusiness = จำนวนลูกค้าตามขนาดธุรกิจ\r\nNumCusTypeBusiness = จำนวนลูกค้าตามประเภทธุรกิจ\r\nNumCusISICCode = จำนวนลูกค้าตาม ISIC Code\r\nNumCusLoanType = จำนวนลูกค้าตามประเภทสินเชื่อ\r\nValueSizeBusiness = มูลค่าสินเชื่อตามขนาดธุรกิจ\r\nValueTypeBusiness = มูลค่าสินเชื่อตามประเภทธุรกิจ\r\nValueISICCode = มูลค่าสินเชื่อตาม ISIC Code\r\nValueLoanType = มูลค่าสินเชื่อตามประเภทสินเชื่อ");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.TitleName).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnType("int(11)");
            entity.Property(e => e.Value).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Dash_Status_Total>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Dash_Status_Total");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.NumCusAll)
                .HasComment("จำนวนลูกค้านำเข้าทั้งหมด")
                .HasColumnType("int(11)");
            entity.Property(e => e.NumCusInProcess)
                .HasComment("อยู่ในกระบวนการ")
                .HasColumnType("int(11)");
            entity.Property(e => e.NumCusMCenterAssign)
                .HasComment("ผู้จัดการศูนย์มอบหมาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.NumCusReturn)
                .HasComment("รายการส่งกลับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.NumCusTargeNotSuccess)
                .HasComment("พนักงานที่ไม่บรรลุเป้าหมาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.NumCusWaitMCenterAssign)
                .HasComment("รอผู้จัดการศูนย์มอบหมาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.Dash_Status_Totals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("dash_status_total_ibfk_1");
        });

        modelBuilder.Entity<FileUpload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("FileUpload");

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasComment("ชื่อไฟล์ที่ใช้ในระบบ");
            entity.Property(e => e.FileSize)
                .HasComment("ขนาดไฟล์")
                .HasColumnType("int(11)");
            entity.Property(e => e.MimeType)
                .HasMaxLength(50)
                .HasComment("นามสกุลไฟล์");
            entity.Property(e => e.OriginalFileName)
                .HasMaxLength(255)
                .HasComment("ชื่อเดิมไฟล์");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Url).HasMaxLength(500);
        });

        modelBuilder.Entity<InfoAmphur>(entity =>
        {
            entity.HasKey(e => e.AmphurID).HasName("PRIMARY");

            entity.ToTable("InfoAmphur", tb => tb.HasComment("ข้อมูลอำเภอ"));

            entity.Property(e => e.AmphurID)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.AmphurCode).HasMaxLength(50);
            entity.Property(e => e.AmphurName).HasMaxLength(255);
            entity.Property(e => e.ProvinceID).HasColumnType("int(11)");
        });

        modelBuilder.Entity<InfoBranch>(entity =>
        {
            entity.HasKey(e => e.BranchID).HasName("PRIMARY");

            entity.ToTable("InfoBranch");

            entity.Property(e => e.BranchID)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.BranchCode).HasMaxLength(255);
            entity.Property(e => e.BranchName).HasMaxLength(255);
            entity.Property(e => e.BranchNameMain).HasMaxLength(255);
            entity.Property(e => e.ProvinceID).HasColumnType("int(11)");
        });

        modelBuilder.Entity<InfoProvince>(entity =>
        {
            entity.HasKey(e => e.ProvinceID).HasName("PRIMARY");

            entity.ToTable("InfoProvince", tb => tb.HasComment("ข้อมูลจังหวัด"));

            entity.HasIndex(e => e.ProvinceID, "ProvinceID");

            entity.Property(e => e.ProvinceID)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Master_Department_BranchId).HasComment("กิจการสาขาภาค");
            entity.Property(e => e.ProvinceCode).HasMaxLength(50);
            entity.Property(e => e.ProvinceName).HasMaxLength(255);
            entity.Property(e => e.RegionID).HasColumnType("int(11)");
        });

        modelBuilder.Entity<InfoTambol>(entity =>
        {
            entity.HasKey(e => e.TambolID).HasName("PRIMARY");

            entity.ToTable("InfoTambol", tb => tb.HasComment("ข้อมูลตำบล"));

            entity.Property(e => e.TambolID)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.AmphurID).HasColumnType("int(11)");
            entity.Property(e => e.ProvinceID).HasColumnType("int(11)");
            entity.Property(e => e.TambolCode).HasMaxLength(50);
            entity.Property(e => e.TambolName).HasMaxLength(255);
            entity.Property(e => e.ZipCode).HasMaxLength(255);
        });

        modelBuilder.Entity<Logging>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("Logging");

            entity.Property(e => e.ClientIp).HasMaxLength(255);
            entity.Property(e => e.ContentType).HasMaxLength(255);
            entity.Property(e => e.DeviceInfo).HasMaxLength(1000);
            entity.Property(e => e.Host).HasMaxLength(100);
            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.Path).HasMaxLength(255);
            entity.Property(e => e.Query).HasMaxLength(500);
            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.ResponseContentType).HasMaxLength(255);
            entity.Property(e => e.ResponseDate).HasColumnType("datetime");
            entity.Property(e => e.ResponseStatus).HasMaxLength(255);
            entity.Property(e => e.Scheme).HasMaxLength(255);
        });

        modelBuilder.Entity<Master_Branch_Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Branch_Region", tb => tb.HasComment("กิจการสาขาภาค"));

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasComment("รหัส");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_BusinessSize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_BusinessSize", tb => tb.HasComment("ขนาดธุรกิจ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_BusinessType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_BusinessType", tb => tb.HasComment("ประเภทกิจการ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Chain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Chain", tb => tb.HasComment("ห่วงโซ่"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_ContactChannel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_ContactChannel", tb => tb.HasComment("ช่องทางการติดต่อ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Department");

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasComment("รหัส");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_ISICCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_ISICCode");

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_List>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_List");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Path).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
        });

        modelBuilder.Entity<Master_LoanType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_LoanType", tb => tb.HasComment("ประเภทสินเชื่อ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Meet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Meet", tb => tb.HasComment("การเข้าพบ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_NextAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_NextAction");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IsNext)
                .HasComment("1=ไปขั้นตอนถัดไป")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Type)
                .HasComment("1=ติดต่อ\r\n,2=เข้าพบ\r\n,3=ยื่นเอกสาร\r\n,4=ผลลัพธ์\r\n")
                .HasColumnType("int(11)");
        });

        modelBuilder.Entity<Master_Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Position", tb => tb.HasComment("ตำแหน่ง"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Type)
                .HasComment("1=Admin 2=User")
                .HasColumnType("int(11)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Proceed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Proceed", tb => tb.HasComment("ดำเนินการ ยื่นเอกสารกู้"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_ProductProgramBank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_ProductProgramBank", tb => tb.HasComment("Product Program ของธนาคาร โครงการสินเชื่อ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_ReasonReturn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_ReasonReturn", tb => tb.HasComment("เหตุผลการส่งคืน"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Reason_CloseSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Reason_CloseSale");

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_SLAOperation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_SLAOperation", tb => tb.HasComment("SLA การดำเนินการ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Day).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_StatusSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_StatusSale", tb => tb.HasComment("สถานะการขาย"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsShowFilter)
                .HasComment("1=แสดงใน filter")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.MainId).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.NameMain).HasMaxLength(255);
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
        });

        modelBuilder.Entity<Master_TSIC>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_TSIC", tb => tb.HasComment("ประเภทธุรกิจ (TSIC)"));

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_TypeLoanRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_TypeLoanRequest", tb => tb.HasComment("ประเภทการขอสินเชื่อ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Year>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Year");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
        });

        modelBuilder.Entity<Master_Yield>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Yield", tb => tb.HasComment("ผลผลิต"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("MenuItem", tb => tb.HasComment("เมนู"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.MenuNumber).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ParentNumber).HasColumnType("int(11)");
            entity.Property(e => e.Path).HasMaxLength(255);
            entity.Property(e => e.Sequence).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Notification");

            entity.HasIndex(e => e.FromUserId, "FromUserId");

            entity.HasIndex(e => e.ToUserId, "ToUserId");

            entity.Property(e => e.ActionName1).HasMaxLength(255);
            entity.Property(e => e.ActionName2).HasMaxLength(255);
            entity.Property(e => e.ActionName3).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.EventId)
                .HasComment("1=รายการลูกค้าใหม่ ,2=อนุมัติคำขอ ,3=ส่งกลับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.FromUserId)
                .HasComment("FK รหัสผู้ใช้ ที่สร้างการแจ้งเตือน")
                .HasColumnType("int(11)");
            entity.Property(e => e.FromUserName).HasMaxLength(255);
            entity.Property(e => e.IsRead)
                .HasComment("0=ยังไม่ได้อ่าน ,1=อ่านแล้ว")
                .HasColumnType("smallint(1)");
            entity.Property(e => e.ReadDate)
                .HasComment("วันที่อ่าน")
                .HasColumnType("datetime");
            entity.Property(e => e.RedirectUrl).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.ToUserId)
                .HasComment("FK รหัสผู้ใช้ ที่จะได้รับการแจ้งเตือน")
                .HasColumnType("int(11)");
            entity.Property(e => e.ToUserName).HasMaxLength(255);

            entity.HasOne(d => d.FromUser).WithMany(p => p.NotificationFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notification_ibfk_1");

            entity.HasOne(d => d.ToUser).WithMany(p => p.NotificationToUsers)
                .HasForeignKey(d => d.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("notification_ibfk_2");
        });

        modelBuilder.Entity<ProcessSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale", tb => tb.HasComment("กระบวนการขาย"));

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<ProcessSale_Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale_Section");

            entity.HasIndex(e => e.ProcessSaleId, "ProcessSaleId");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.ShowAlways)
                .HasComment("1=แสดงผลตลอด")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.ProcessSale).WithMany(p => p.ProcessSale_Sections)
                .HasForeignKey(d => d.ProcessSaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_section_ibfk_1");
        });

        modelBuilder.Entity<ProcessSale_Section_Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale_Section_Item");

            entity.HasIndex(e => e.PSaleSectionId, "ProcessSaleId");

            entity.Property(e => e.ItemLabel).HasMaxLength(255);
            entity.Property(e => e.ItemType).HasMaxLength(255);
            entity.Property(e => e.Required).HasComment("0=ไม่จำเป็น ,1=จำเป็น");
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.ShowType)
                .HasComment("0=แสดงครึ่ง ,1=แสดงเต็ม")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.PSaleSection).WithMany(p => p.ProcessSale_Section_Items)
                .HasForeignKey(d => d.PSaleSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_section_item_ibfk_1");
        });

        modelBuilder.Entity<ProcessSale_Section_ItemOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale_Section_ItemOption");

            entity.HasIndex(e => e.Master_ListId, "Master_ListId");

            entity.HasIndex(e => e.PSaleSectionItemId, "ProcessSaleItemId");

            entity.HasIndex(e => e.ShowSectionId, "ShowSectionId");

            entity.Property(e => e.DefaultValue).HasMaxLength(255);
            entity.Property(e => e.OptionLabel).HasMaxLength(255);
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.ShowSectionId).HasComment("แสดงผลตาม Section (ไม่ต้องผูก FK เพราะ sec save ทีหลัง option)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Master_List).WithMany(p => p.ProcessSale_Section_ItemOptions)
                .HasForeignKey(d => d.Master_ListId)
                .HasConstraintName("processsale_section_itemoption_ibfk_2");

            entity.HasOne(d => d.PSaleSectionItem).WithMany(p => p.ProcessSale_Section_ItemOptions)
                .HasForeignKey(d => d.PSaleSectionItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_section_itemoption_ibfk_1");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale", tb => tb.HasComment("การขาย"));

            entity.HasIndex(e => e.AssCenterUserId, "AssignedCenterUserId");

            entity.HasIndex(e => e.AssUserId, "AssignedUserId");

            entity.HasIndex(e => e.BranchId, "BranchId");

            entity.HasIndex(e => e.CustomerId, "CustomerId");

            entity.HasIndex(e => e.Master_Branch_RegionId, "Master_Branch_RegionId");

            entity.HasIndex(e => e.Master_Reason_CloseSaleId, "Master_Reason_CloseSaleId");

            entity.HasIndex(e => e.StatusSaleId, "StatusSaleId");

            entity.Property(e => e.AssCenterCreateBy)
                .HasComment("ผู้มอบหมายผู้จัดการศูนย์")
                .HasColumnType("int(11)");
            entity.Property(e => e.AssCenterDate)
                .HasComment("วันที่มอบหมายผู้จัดการศูนย์")
                .HasColumnType("datetime");
            entity.Property(e => e.AssCenterUserId)
                .HasComment("ผู้จัดการศูนย์ที่ดูแล")
                .HasColumnType("int(11)");
            entity.Property(e => e.AssCenterUserName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้จัดการศูนย์ที่ดูแล");
            entity.Property(e => e.AssUserId)
                .HasComment("พนักงานที่ได้รับมอบหมาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.AssUserName)
                .HasMaxLength(255)
                .HasComment("ชื่อพนักงานที่ได้รับมอบหมาย");
            entity.Property(e => e.BranchId)
                .HasComment("สาขา")
                .HasColumnType("int(11)");
            entity.Property(e => e.BranchName).HasMaxLength(255);
            entity.Property(e => e.CIF).HasMaxLength(255);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasComment("ชื่อบริษัท");
            entity.Property(e => e.ContactStartDate)
                .HasComment("วันที่เริ่มติดต่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateByName).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DateAppointment)
                .HasComment("วันที่นัดหมาย")
                .HasColumnType("datetime");
            entity.Property(e => e.LoanAmount)
                .HasPrecision(18, 2)
                .HasComment("จำนวนการกู้");
            entity.Property(e => e.Master_Branch_RegionId).HasComment("กิจการสาขาภาค");
            entity.Property(e => e.Master_Branch_RegionName).HasMaxLength(255);
            entity.Property(e => e.Master_Reason_CloseSaleId).HasComment("เหตุผลไม่ประสงค์กู้");
            entity.Property(e => e.PercentChanceLoanPass)
                .HasComment("เปอร์เซ็นโอกาสกู้ผ่าน")
                .HasColumnType("int(11)");
            entity.Property(e => e.ProvinceId)
                .HasComment("จังหวัด")
                .HasColumnType("int(11)");
            entity.Property(e => e.ProvinceName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(255)
                .HasComment("รายละเอียดสถานะ");
            entity.Property(e => e.StatusSaleId).HasColumnType("int(11)");
            entity.Property(e => e.StatusSaleMainId).HasColumnType("int(11)");
            entity.Property(e => e.StatusSaleName)
                .HasMaxLength(255)
                .HasComment("สถานะการขาย");
            entity.Property(e => e.StatusSaleNameMain)
                .HasMaxLength(255)
                .HasComment("สถานะการขายหลัก");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateByName).HasMaxLength(255);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.AssCenterUser).WithMany(p => p.SaleAssCenterUsers)
                .HasForeignKey(d => d.AssCenterUserId)
                .HasConstraintName("sale_ibfk_3");

            entity.HasOne(d => d.AssUser).WithMany(p => p.SaleAssUsers)
                .HasForeignKey(d => d.AssUserId)
                .HasConstraintName("sale_ibfk_4");

            entity.HasOne(d => d.Branch).WithMany(p => p.Sales)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("sale_ibfk_8");

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_ibfk_1");

            entity.HasOne(d => d.Master_Branch_Region).WithMany(p => p.Sales)
                .HasForeignKey(d => d.Master_Branch_RegionId)
                .HasConstraintName("sale_ibfk_7");

            entity.HasOne(d => d.Master_Reason_CloseSale).WithMany(p => p.Sales)
                .HasForeignKey(d => d.Master_Reason_CloseSaleId)
                .HasConstraintName("sale_ibfk_6");

            entity.HasOne(d => d.StatusSale).WithMany(p => p.Sales)
                .HasForeignKey(d => d.StatusSaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_ibfk_2");
        });

        modelBuilder.Entity<Sale_Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Activity", tb => tb.HasComment("จำนวนครั้งการดำเนินการแต่ละขั้นตอน"));

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CloseSale)
                .HasComment("ปิดการขาย(ครั้ง)")
                .HasColumnType("int(11)");
            entity.Property(e => e.Contact)
                .HasComment("ติดต่อ(ครั้ง)")
                .HasColumnType("int(11)");
            entity.Property(e => e.ContactName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Document)
                .HasComment("ยื่นเอกสาร(ครั้ง)")
                .HasColumnType("int(11)");
            entity.Property(e => e.Meet)
                .HasComment("เข้าพบ(ครั้ง)")
                .HasColumnType("int(11)");
            entity.Property(e => e.Result)
                .HasComment("ผลลัพธ์(ครั้ง)")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Activities)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_activity_ibfk_1");
        });

        modelBuilder.Entity<Sale_Close_Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Close_Sale");

            entity.HasIndex(e => e.Master_Reason_CloseSaleId, "Master_Reason_CloseSaleId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DesireLoanId)
                .HasComment("1=ประสงค์กู้ 2=ไม่ประสงค์กู้")
                .HasColumnType("int(11)");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.Note)
                .HasMaxLength(1000)
                .HasComment("บันทึกเพิ่มเติม");
            entity.Property(e => e.ResultMeetId)
                .HasComment("1=รับสาย 2=ไม่รับสาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel)
                .HasMaxLength(255)
                .HasComment("เบอร์ติดต่อ");

            entity.HasOne(d => d.Master_Reason_CloseSale).WithMany(p => p.Sale_Close_Sales)
                .HasForeignKey(d => d.Master_Reason_CloseSaleId)
                .HasConstraintName("sale_close_sale_ibfk_2");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Close_Sales)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_close_sale_ibfk_1");
        });

        modelBuilder.Entity<Sale_Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Contact", tb => tb.HasComment("ติดต่อ"));

            entity.HasIndex(e => e.NextActionId, "Master_NextActionId");

            entity.HasIndex(e => e.Master_Reason_CloseSaleId, "Master_Reason_CloseSaleId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.AppointmentDate)
                .HasComment("วันที่นัดหมาย")
                .HasColumnType("datetime");
            entity.Property(e => e.AppointmentTime)
                .HasComment("เวลาที่นัดหมาย")
                .HasColumnType("time");
            entity.Property(e => e.ContactDate)
                .HasComment("วันที่ติดต่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.ContactResult)
                .HasComment("1=รับสาย 2=ไม่รับสาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DesireLoanId)
                .HasComment("2=ไม่ประสงค์กู้")
                .HasColumnType("int(11)");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasComment("สถานที่");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.NextActionId)
                .HasComment("1=ทำการนัดหมาย 2=ติดต่ออีกครั้ง 3=ส่งกลับรายการ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Note)
                .HasMaxLength(1000)
                .HasComment("บันทึกเพิ่มเติม");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel)
                .HasMaxLength(255)
                .HasComment("เบอร์ติดต่อ");

            entity.HasOne(d => d.Master_Reason_CloseSale).WithMany(p => p.Sale_Contacts)
                .HasForeignKey(d => d.Master_Reason_CloseSaleId)
                .HasConstraintName("sale_contact_ibfk_2");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Contacts)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_contact_ibfk_1");
        });

        modelBuilder.Entity<Sale_Contact_History>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Contact_History", tb => tb.HasComment("ประวัติการติดต่อ"));

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.HasIndex(e => e.StatusSaleId, "StatusSaleId");

            entity.Property(e => e.AppointmentDate)
                .HasComment("วันที่นัดหมาย")
                .HasColumnType("datetime");
            entity.Property(e => e.AppointmentTime)
                .HasComment("เวลาที่นัดหมาย")
                .HasColumnType("time");
            entity.Property(e => e.ContactDate)
                .HasComment("วันที่ติดต่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.ContactFullName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.ContactTel)
                .HasMaxLength(255)
                .HasComment("เบอร์ติดต่อ");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateByName).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreditLimit)
                .HasPrecision(18, 2)
                .HasComment("วงเงิน");
            entity.Property(e => e.DesireLoanName)
                .HasMaxLength(255)
                .HasComment("ความประสงค์กู้");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasComment("สถานที่");
            entity.Property(e => e.MeetFullName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้เข้าพบ");
            entity.Property(e => e.NextActionName)
                .HasMaxLength(255)
                .HasComment("Next Action");
            entity.Property(e => e.Note)
                .HasMaxLength(1000)
                .HasComment("บันทึกเพิ่มเติม");
            entity.Property(e => e.NoteSystem)
                .HasMaxLength(255)
                .HasComment("Note system");
            entity.Property(e => e.Percent)
                .HasMaxLength(255)
                .HasComment("ร้อยละ");
            entity.Property(e => e.ProceedName)
                .HasMaxLength(255)
                .HasComment("ชื่อการดำเนินการ");
            entity.Property(e => e.ProcessSaleCode).HasMaxLength(255);
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasComment("เหตุผล");
            entity.Property(e => e.ResultContactName)
                .HasMaxLength(255)
                .HasComment("ผลการติดต่อ");
            entity.Property(e => e.ResultMeetName)
                .HasMaxLength(255)
                .HasComment("ผลการเข้าพบ");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.StatusName)
                .HasMaxLength(255)
                .HasComment("สถานะ");
            entity.Property(e => e.StatusSaleId).HasColumnType("int(11)");
            entity.Property(e => e.TopicName)
                .HasMaxLength(255)
                .HasComment("ชื่อหัวข้อ");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Contact_Histories)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_contact_history_ibfk_1");

            entity.HasOne(d => d.StatusSale).WithMany(p => p.Sale_Contact_Histories)
                .HasForeignKey(d => d.StatusSaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_contact_history_ibfk_2");
        });

        modelBuilder.Entity<Sale_Contact_Info>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Contact_Info", tb => tb.HasComment("ข้อมูลผู้ติดต่อ"));

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.Position)
                .HasMaxLength(255)
                .HasComment("ตำแหน่ง");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel).HasMaxLength(255);

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Contact_Infos)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_contact_info_ibfk_1");
        });

        modelBuilder.Entity<Sale_Deliver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Deliver", tb => tb.HasComment("ระยะเวลาในการส่งมอบ"));

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.BranchRegToCenBranch)
                .HasComment("กิจการสาขาภาคมอบหมายผู้จัดการศูนย์สาขา")
                .HasColumnType("int(11)");
            entity.Property(e => e.CenBranchToRM)
                .HasComment("ผู้จัดการศูนย์สาขามอบหมายพนักงาน RM")
                .HasColumnType("int(11)");
            entity.Property(e => e.CloseSale)
                .HasComment("ปิดการขาย(ครั้ง)")
                .HasColumnType("int(11)");
            entity.Property(e => e.ContactName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LoanToBranchReg)
                .HasComment("ศูนย์ธุระกิจสินเชื่อมองหมายกิจการสาขาภาค")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Delivers)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_deliver_ibfk_1");
        });

        modelBuilder.Entity<Sale_Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Document");

            entity.HasIndex(e => e.HouseRegistrationFileId, "HouseRegistrationFileId");

            entity.HasIndex(e => e.Master_ProductProgramBankId, "Master_ProductProgramBankId");

            entity.HasIndex(e => e.Master_TypeLoanRequestId, "Master_TypeLoanRequest");

            entity.HasIndex(e => e.OtherDocumentFileId, "OtherDocumentFileId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.HasIndex(e => e.SignatureEmployeeFileId, "SignatureEmployeeFileId");

            entity.HasIndex(e => e.SignatureFileId, "SignatureFileId");

            entity.HasIndex(e => e.SignatureMCenterFileId, "SignatureMCenterFileId");

            entity.Property(e => e.AmphurId)
                .HasComment("อำเภอ")
                .HasColumnType("int(11)");
            entity.Property(e => e.AmphurName).HasMaxLength(255);
            entity.Property(e => e.Birthday)
                .HasComment("วันเกิด")
                .HasColumnType("datetime");
            entity.Property(e => e.BusinessOperation)
                .HasMaxLength(1000)
                .HasComment("ลักษณะการดำเนินธุรกิจ");
            entity.Property(e => e.CommentEmployeeLoan)
                .HasMaxLength(500)
                .HasComment("ความคิดเห็นพนักงานสินเชื่อ");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DateFirstContactBank)
                .HasComment("วันที่เริ่มติดต่อกับธนาคารในการขอกู้ครั้งนี้")
                .HasColumnType("datetime");
            entity.Property(e => e.HouseNo)
                .HasMaxLength(255)
                .HasComment("บ้านเลขที่");
            entity.Property(e => e.HouseRegistrationFileId).HasComment("id file ไฟล์ทะเบียนนบ้าน");
            entity.Property(e => e.HouseRegistrationPath)
                .HasMaxLength(255)
                .HasComment("ไฟล์ทะเบียนนบ้าน");
            entity.Property(e => e.IDCardIMGPath)
                .HasMaxLength(255)
                .HasComment("รูปบัตรประชาชน");
            entity.Property(e => e.IDCardNumber)
                .HasMaxLength(20)
                .HasComment("เลขบัตรประชาชน");
            entity.Property(e => e.LoanLimitBusiness)
                .HasPrecision(18, 2)
                .HasComment("วงเงินกู้สำหรับเงินทุนหมุนเวียนในกิจการ");
            entity.Property(e => e.LoanLimitInvestmentCost)
                .HasPrecision(18, 2)
                .HasComment("วงเงินกู้สำหรับค่าลงทุน");
            entity.Property(e => e.LoanLimitObjectiveOther)
                .HasPrecision(18, 2)
                .HasComment("วงเงินกู้สำหรับวัตถุประสงค์");
            entity.Property(e => e.LoanLimitObjectiveOtherSpecify)
                .HasMaxLength(255)
                .HasComment("ระบุ");
            entity.Property(e => e.Master_BusinessTypeId).HasComment("ประเภทธุรกิจ");
            entity.Property(e => e.Master_TypeLoanRequestSpecify)
                .HasMaxLength(255)
                .HasComment("ระบุ");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasComment("ชื่อภาษาอังกฤษ");
            entity.Property(e => e.NameTh)
                .HasMaxLength(255)
                .HasComment("ชื่อภาษาไทย");
            entity.Property(e => e.OtherDocumentFileId).HasComment("id file ไฟล์เอกสารอื่นๆ");
            entity.Property(e => e.OtherDocumentPath)
                .HasMaxLength(255)
                .HasComment("ไฟล์เอกสารอื่นๆ");
            entity.Property(e => e.ProvinceId)
                .HasComment("จังหวัด")
                .HasColumnType("int(11)");
            entity.Property(e => e.ProvinceName).HasMaxLength(255);
            entity.Property(e => e.RegistrationDate)
                .HasComment("วันที่รับขึ้นทะเบียนเป็นลูกค้า")
                .HasColumnType("datetime");
            entity.Property(e => e.Religion)
                .HasMaxLength(255)
                .HasComment("ศาสนา");
            entity.Property(e => e.SignatureDate)
                .HasComment("วันที่เซ็นผู้กู้ยืม")
                .HasColumnType("datetime");
            entity.Property(e => e.SignatureEmployeeFileId).HasComment("id file รูปลายเซ็นพนักงานสินเชื่อ");
            entity.Property(e => e.SignatureEmployeeLoanDate)
                .HasComment("วันที่เซ็นพนักงานสินเชื่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.SignatureEmployeeLoanPath)
                .HasMaxLength(255)
                .HasComment("รูปลายเซ็นพนักงานสินเชื่อ");
            entity.Property(e => e.SignatureFileId).HasComment("id file รูปลายเซ็นผู้กู้ยืม");
            entity.Property(e => e.SignatureMCenterDate)
                .HasComment("วันที่เซ็นผู้จัดการศูนย์")
                .HasColumnType("datetime");
            entity.Property(e => e.SignatureMCenterFileId).HasComment("id file รูปลายเซ็นผู้จัดการศูนย์");
            entity.Property(e => e.SignatureMCenterPath)
                .HasMaxLength(255)
                .HasComment("รูปลายเซ็นผู้จัดการศูนย์");
            entity.Property(e => e.SignaturePath)
                .HasMaxLength(255)
                .HasComment("รูปลายเซ็นผู้กู้ยืม");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.SubmitDate)
                .HasComment("วันที่ยื่นกู้")
                .HasColumnType("datetime");
            entity.Property(e => e.SubmitType)
                .HasComment("1=ยื่นกู้")
                .HasColumnType("int(11)");
            entity.Property(e => e.TotaLlimit)
                .HasPrecision(18, 2)
                .HasComment("วงเงินรวม");
            entity.Property(e => e.TotaLlimitCEQA)
                .HasPrecision(18, 2)
                .HasComment("CEQA รวมวงเงินเทียบเท่าสินเชื่อ เท่ากับ");
            entity.Property(e => e.VillageNo)
                .HasMaxLength(255)
                .HasComment("หมู่ที่");

            entity.HasOne(d => d.HouseRegistrationFile).WithMany(p => p.Sale_DocumentHouseRegistrationFiles)
                .HasForeignKey(d => d.HouseRegistrationFileId)
                .HasConstraintName("sale_document_ibfk_7");

            entity.HasOne(d => d.Master_ProductProgramBank).WithMany(p => p.Sale_Documents)
                .HasForeignKey(d => d.Master_ProductProgramBankId)
                .HasConstraintName("sale_document_ibfk_3");

            entity.HasOne(d => d.Master_TypeLoanRequest).WithMany(p => p.Sale_Documents)
                .HasForeignKey(d => d.Master_TypeLoanRequestId)
                .HasConstraintName("sale_document_ibfk_2");

            entity.HasOne(d => d.OtherDocumentFile).WithMany(p => p.Sale_DocumentOtherDocumentFiles)
                .HasForeignKey(d => d.OtherDocumentFileId)
                .HasConstraintName("sale_document_ibfk_8");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Documents)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_document_ibfk_1");

            entity.HasOne(d => d.SignatureEmployeeFile).WithMany(p => p.Sale_DocumentSignatureEmployeeFiles)
                .HasForeignKey(d => d.SignatureEmployeeFileId)
                .HasConstraintName("sale_document_ibfk_5");

            entity.HasOne(d => d.SignatureFile).WithMany(p => p.Sale_DocumentSignatureFiles)
                .HasForeignKey(d => d.SignatureFileId)
                .HasConstraintName("sale_document_ibfk_4");

            entity.HasOne(d => d.SignatureMCenterFile).WithMany(p => p.Sale_DocumentSignatureMCenterFiles)
                .HasForeignKey(d => d.SignatureMCenterFileId)
                .HasConstraintName("sale_document_ibfk_6");
        });

        modelBuilder.Entity<Sale_Duration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Duration", tb => tb.HasComment("จำนวนวันการดำเนินการแต่ละขั้นตอน"));

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CloseSale)
                .HasComment("ปิดการขาย(วัน)")
                .HasColumnType("int(11)");
            entity.Property(e => e.Contact)
                .HasComment("ติดต่อ(วัน)")
                .HasColumnType("int(11)");
            entity.Property(e => e.ContactName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.ContactStartDate)
                .HasComment("วันที่เริ่มติดต่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Document)
                .HasComment("พิจารณาเอกสาร(วัน)")
                .HasColumnType("int(11)");
            entity.Property(e => e.Meet)
                .HasComment("เข้าพบ(วัน)")
                .HasColumnType("int(11)");
            entity.Property(e => e.Result)
                .HasComment("ผลลัพธ์(วัน)")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.TotalDay)
                .HasComment("รวม")
                .HasColumnType("int(11)");
            entity.Property(e => e.WaitContact)
                .HasComment("รอการติดต่อ(วัน)")
                .HasColumnType("int(11)");
            entity.Property(e => e.WaitMeet)
                .HasComment("รอเข้าพบ(วัน)")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Durations)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_duration_ibfk_1");
        });

        modelBuilder.Entity<Sale_Meet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Meet", tb => tb.HasComment("เข้าพบ"));

            entity.HasIndex(e => e.NextActionId, "Master_NextActionId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.AppointmentDate)
                .HasComment("วันที่นัดหมาย")
                .HasColumnType("datetime");
            entity.Property(e => e.AppointmentTime)
                .HasComment("เวลาที่นัดหมาย")
                .HasColumnType("time");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LoanAmount)
                .HasPrecision(18, 2)
                .HasComment("จำนวนการกู้");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasComment("สถานที่");
            entity.Property(e => e.Master_ChainId).HasComment("ห่วงโซ่คุณค่า ");
            entity.Property(e => e.Master_YieldId).HasComment("ผลผลิตหลัก");
            entity.Property(e => e.MeetDate)
                .HasComment("วันที่เข้าพบ")
                .HasColumnType("datetime");
            entity.Property(e => e.MeetId)
                .HasComment("1=เข้าพบสำเร็จ 2=เข้าพบไม่สำเร็จ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("บุคคลที่เข้าพบ");
            entity.Property(e => e.NextActionId)
                .HasComment("1=นัดเก็บเอกสาร/ประสงค์กู้ 2=เข้าพบอีกครั้ง")
                .HasColumnType("int(11)");
            entity.Property(e => e.Note)
                .HasMaxLength(1000)
                .HasComment("บันทึกเพิ่มเติม");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel)
                .HasMaxLength(255)
                .HasComment("เบอร์ติดต่อ");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Meets)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_meet_ibfk_1");
        });

        modelBuilder.Entity<Sale_Partner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Partner", tb => tb.HasComment("คู่ค้า"));

            entity.HasIndex(e => e.Master_BusinessSizeId, "Master_BusinessSizeId");

            entity.HasIndex(e => e.Master_BusinessTypeId, "Master_BusinessTypeId");

            entity.HasIndex(e => e.Master_ChainId, "Master_ChainId");

            entity.HasIndex(e => e.Master_YieldId, "Master_YieldId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasComment("ชื่อคู่ค้า");
            entity.Property(e => e.Master_BusinessSizeId).HasComment("ขนาดธุรกิจ");
            entity.Property(e => e.Master_BusinessSizeName).HasMaxLength(255);
            entity.Property(e => e.Master_BusinessTypeId).HasComment("ประเภทธุรกิจ");
            entity.Property(e => e.Master_BusinessTypeName).HasMaxLength(255);
            entity.Property(e => e.Master_ChainId).HasComment("ห่วงโซ่คุณค่า ");
            entity.Property(e => e.Master_ChainName).HasMaxLength(255);
            entity.Property(e => e.Master_YieldId).HasComment("ผลผลิตหลัก");
            entity.Property(e => e.Master_YieldName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel).HasMaxLength(255);

            entity.HasOne(d => d.Master_BusinessSize).WithMany(p => p.Sale_Partners)
                .HasForeignKey(d => d.Master_BusinessSizeId)
                .HasConstraintName("sale_partner_ibfk_5");

            entity.HasOne(d => d.Master_BusinessType).WithMany(p => p.Sale_Partners)
                .HasForeignKey(d => d.Master_BusinessTypeId)
                .HasConstraintName("sale_partner_ibfk_2");

            entity.HasOne(d => d.Master_Chain).WithMany(p => p.Sale_Partners)
                .HasForeignKey(d => d.Master_ChainId)
                .HasConstraintName("sale_partner_ibfk_4");

            entity.HasOne(d => d.Master_Yield).WithMany(p => p.Sale_Partners)
                .HasForeignKey(d => d.Master_YieldId)
                .HasConstraintName("sale_partner_ibfk_3");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Partners)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_partner_ibfk_1");
        });

        modelBuilder.Entity<Sale_Reply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Reply");

            entity.HasIndex(e => e.ProcessSaleId, "ProcessSaleItemId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateByName).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ProcessSaleName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateByName).HasMaxLength(255);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.ProcessSale).WithMany(p => p.Sale_Replies)
                .HasForeignKey(d => d.ProcessSaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_reply_ibfk_2");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Replies)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_reply_ibfk_1");
        });

        modelBuilder.Entity<Sale_Reply_Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Reply_Section");

            entity.HasIndex(e => e.SaleReplyId, "PSaleReplyId");

            entity.HasIndex(e => e.PSaleSectionId, "PSaleSectionId");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.PSaleSection).WithMany(p => p.Sale_Reply_Sections)
                .HasForeignKey(d => d.PSaleSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_reply_section_ibfk_2");

            entity.HasOne(d => d.SaleReply).WithMany(p => p.Sale_Reply_Sections)
                .HasForeignKey(d => d.SaleReplyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_reply_section_ibfk_1");
        });

        modelBuilder.Entity<Sale_Reply_Section_Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Reply_Section_Item");

            entity.HasIndex(e => e.SaleReplySectionId, "ProcessSaleReplyId");

            entity.HasIndex(e => e.PSaleSectionItemId, "processsale_answeritem_ibfk_2");

            entity.Property(e => e.ItemLabel).HasMaxLength(255);
            entity.Property(e => e.ItemType).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.PSaleSectionItem).WithMany(p => p.Sale_Reply_Section_Items)
                .HasForeignKey(d => d.PSaleSectionItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_reply_section_item_ibfk_2");

            entity.HasOne(d => d.SaleReplySection).WithMany(p => p.Sale_Reply_Section_Items)
                .HasForeignKey(d => d.SaleReplySectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_reply_section_item_ibfk_1");
        });

        modelBuilder.Entity<Sale_Reply_Section_ItemValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Reply_Section_ItemValue");

            entity.HasIndex(e => e.FileId, "FileId");

            entity.HasIndex(e => e.SaleReplySectionItemId, "PSaleReplySectionItemId");

            entity.HasIndex(e => e.PSaleSectionItemOptionId, "PSaleSectionItemOptionId");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FileUrl).HasMaxLength(255);
            entity.Property(e => e.OptionLabel).HasMaxLength(255);
            entity.Property(e => e.ReplyDate).HasColumnType("datetime");
            entity.Property(e => e.ReplyName).HasMaxLength(1000);
            entity.Property(e => e.ReplyTime).HasColumnType("time");
            entity.Property(e => e.ReplyValue).HasMaxLength(1000);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.File).WithMany(p => p.Sale_Reply_Section_ItemValues)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("sale_reply_section_itemvalue_ibfk_3");

            entity.HasOne(d => d.PSaleSectionItemOption).WithMany(p => p.Sale_Reply_Section_ItemValues)
                .HasForeignKey(d => d.PSaleSectionItemOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_reply_section_itemvalue_ibfk_2");

            entity.HasOne(d => d.SaleReplySectionItem).WithMany(p => p.Sale_Reply_Section_ItemValues)
                .HasForeignKey(d => d.SaleReplySectionItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_reply_section_itemvalue_ibfk_1");
        });

        modelBuilder.Entity<Sale_Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Result");

            entity.HasIndex(e => e.Master_ContactChannelId, "Master_ContactChannelId");

            entity.HasIndex(e => e.ProceedId, "Master_ProceedId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.AppointmentDate)
                .HasComment("วันที่นัดหมาย")
                .HasColumnType("datetime");
            entity.Property(e => e.AppointmentTime)
                .HasComment("เวลาที่นัดหมาย")
                .HasColumnType("time");
            entity.Property(e => e.AttachmentPath)
                .HasMaxLength(255)
                .HasComment("เอกสาร");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DateContact)
                .HasComment("วันที่ติดต่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasComment("สถานที่");
            entity.Property(e => e.Master_ContactChannelId).HasComment("ช่องทางการติดต่อ");
            entity.Property(e => e.MeetName)
                .HasMaxLength(255)
                .HasComment("ผู้เข้าพบ");
            entity.Property(e => e.NextActionId)
                .HasComment("1=ทำการนัดหมาย 2=รอปิดการขาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.Note)
                .HasMaxLength(1000)
                .HasComment("บันทึกเพิ่มเติม");
            entity.Property(e => e.ProceedId)
                .HasComment("1=แจ้งข้อมูลเพิ่มเติม 2=ติดต่อขอเอกสาร 3=เข้าพบรับเอกสาร 4=ไม่ผ่านการพิจารณา")
                .HasColumnType("int(11)");
            entity.Property(e => e.ResultMeetId)
                .HasComment("1=เข้าพบสำเร็จ 2=เข้าพบไม่สำเร็จ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel)
                .HasMaxLength(255)
                .HasComment("เบอร์โทร");

            entity.HasOne(d => d.Master_ContactChannel).WithMany(p => p.Sale_Results)
                .HasForeignKey(d => d.Master_ContactChannelId)
                .HasConstraintName("sale_result_ibfk_2");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Results)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_result_ibfk_1");
        });

        modelBuilder.Entity<Sale_Return>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Return");

            entity.Property(e => e.AssUserId)
                .HasComment("พนักงานที่ได้รับมอบหมาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.AssUserName)
                .HasMaxLength(255)
                .HasComment("ชื่อพนักงานที่ได้รับมอบหมาย");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasComment("ชื่อบริษัท");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateByName).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Master_BusinessTypeId).HasComment("ประเภทธุรกิจ");
            entity.Property(e => e.Master_BusinessTypeName).HasMaxLength(255);
            entity.Property(e => e.Master_LoanTypeId).HasComment("ประเภทสินเชื่อ");
            entity.Property(e => e.Master_LoanTypeName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.StatusDescription)
                .HasMaxLength(255)
                .HasComment("รายละเอียดสถานะ");
            entity.Property(e => e.StatusSaleId).HasColumnType("int(11)");
            entity.Property(e => e.StatusSaleName)
                .HasMaxLength(255)
                .HasComment("สถานะการขาย");
        });

        modelBuilder.Entity<Sale_Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Status", tb => tb.HasComment("สถานะการขาย"));

            entity.HasIndex(e => e.CreateBy, "CreateBy");

            entity.HasIndex(e => e.Master_Reason_CloseSaleId, "Master_Reason_CloseSaleId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.HasIndex(e => e.StatusId, "StatusId");

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateByName).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.StatusId).HasColumnType("int(11)");
            entity.Property(e => e.StatusMainId).HasColumnType("int(11)");
            entity.Property(e => e.StatusName)
                .HasMaxLength(255)
                .HasComment("สถานะการขาย");
            entity.Property(e => e.StatusNameMain)
                .HasMaxLength(255)
                .HasComment("สถานะการขายหลัก");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.Sale_Statuses)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_status_ibfk_3");

            entity.HasOne(d => d.Master_Reason_CloseSale).WithMany(p => p.Sale_Statuses)
                .HasForeignKey(d => d.Master_Reason_CloseSaleId)
                .HasConstraintName("sale_status_ibfk_4");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Statuses)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_status_ibfk_1");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Sale_Statuses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_status_ibfk_2");
        });

        modelBuilder.Entity<Sale_Status_Total>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Status_Total");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.AllCustomer)
                .HasComment("ลูกค้าทั้งหมด")
                .HasColumnType("int(11)");
            entity.Property(e => e.CloseSale)
                .HasComment("ปิดการขาย")
                .HasColumnType("int(11)");
            entity.Property(e => e.Contact)
                .HasComment("ติดต่อ")
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Meet)
                .HasComment("เข้าพบ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Results)
                .HasComment("ผลลัพธ์")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.SubmitDocument)
                .HasComment("ยื่นเอกสาร")
                .HasColumnType("int(11)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");
            entity.Property(e => e.WaitContact)
                .HasComment("รอการติดต่อ")
                .HasColumnType("int(11)");
            entity.Property(e => e.WaitMeet)
                .HasComment("รอการเข้าพบ")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.Sale_Status_Totals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_status_total_ibfk_1");
        });

        modelBuilder.Entity<SendMail_Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("SendMail_Log");

            entity.HasIndex(e => e.SendMail_TemplateId, "SendMail_TemplateId");

            entity.Property(e => e.CreateById)
                .HasComment("ผู้สร้าง/เป็น null ได้กรณีคนนอกไม่ได้ login")
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateDate)
                .HasComment("วันที่สร้าง")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailTo).HasMaxLength(100);
            entity.Property(e => e.EmailToCc).HasMaxLength(1000);
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.StatusMessage).HasColumnType("text");
            entity.Property(e => e.Subject).HasMaxLength(300);

            entity.HasOne(d => d.SendMail_Template).WithMany(p => p.SendMail_Logs)
                .HasForeignKey(d => d.SendMail_TemplateId)
                .HasConstraintName("sendmail_log_ibfk_1");
        });

        modelBuilder.Entity<SendMail_Template>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("SendMail_Template");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreateDate)
                .HasComment("วันที่สร้าง")
                .HasColumnType("datetime");
            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Subject).HasMaxLength(300);
        });

        modelBuilder.Entity<System_SLA>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("System_SLA", tb => tb.HasComment("SLA การติดต่อ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.NumberDays).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<System_Signature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("System_Signature", tb => tb.HasComment("ลายเซ็นอนุมัติ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImgThumbnailUrl)
                .HasMaxLength(255)
                .HasComment("Url รูปขนาดย่อ");
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasComment("Url รูป");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User", tb => tb.HasComment("ผู้ใช้งาน"));

            entity.HasIndex(e => e.BranchId, "BranchId");

            entity.HasIndex(e => e.LevelId, "LevelId");

            entity.HasIndex(e => e.Master_Branch_RegionId, "Master_Branch_Region");

            entity.HasIndex(e => e.Master_DepartmentId, "Master_DepartmentId");

            entity.HasIndex(e => e.PositionId, "PositionId");

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.AmphurId)
                .HasComment("อำเภอ")
                .HasColumnType("int(11)");
            entity.Property(e => e.AmphurName).HasMaxLength(255);
            entity.Property(e => e.BranchId)
                .HasComment("สาขา")
                .HasColumnType("int(11)");
            entity.Property(e => e.BranchName).HasMaxLength(255);
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(10)
                .HasComment("รหัสพนักงาน");
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IsSentMail).HasColumnType("smallint(6)");
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.LevelId)
                .HasComment("ระดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.LoginFail).HasColumnType("smallint(6)");
            entity.Property(e => e.Master_Branch_RegionId).HasComment("กิจการสาขาภาค");
            entity.Property(e => e.Master_DepartmentId).HasComment("ฝ่ายส่วนงานธุรกิจสินเชื่อ");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PositionId)
                .HasComment("ตำแหน่ง")
                .HasColumnType("int(11)");
            entity.Property(e => e.ProvinceId)
                .HasComment("จังหวัด")
                .HasColumnType("int(11)");
            entity.Property(e => e.ProvinceName).HasMaxLength(255);
            entity.Property(e => e.RoleId)
                .HasComment("ระดับหน้าที่")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Tel)
                .HasMaxLength(255)
                .HasComment("เบอร์โทร");
            entity.Property(e => e.TitleName).HasMaxLength(255);
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Level).WithMany(p => p.Users)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.Master_Branch_Region).WithMany(p => p.Users)
                .HasForeignKey(d => d.Master_Branch_RegionId)
                .HasConstraintName("user_ibfk_5");

            entity.HasOne(d => d.Master_Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.Master_DepartmentId)
                .HasConstraintName("user_ibfk_6");

            entity.HasOne(d => d.Position).WithMany(p => p.Users)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("user_ibfk_3");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<User_Level>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_Level", tb => tb.HasComment("ระดับ"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
        });

        modelBuilder.Entity<User_Login_Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_Login_Log");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IPAddress).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnType("int(11)");
        });

        modelBuilder.Entity<User_Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_Permission", tb => tb.HasComment("สิทธิ์การเข้าถึง"));

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.MenuNumber).HasColumnType("int(11)");
            entity.Property(e => e.RoleId).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Role).WithMany(p => p.User_Permissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_permission_ibfk_1");
        });

        modelBuilder.Entity<User_Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_Role", tb => tb.HasComment("ระดับหน้าที่"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasComment("รายละเอียดหน้าที่");
            entity.Property(e => e.IsModify).HasComment("อนุญาตให้แก้ไข");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อหน้าที่");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User_Target_Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_Target_Sale", tb => tb.HasComment("เป้ายอดการขาย"));

            entity.HasIndex(e => e.CreateBy, "CreateBy");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.AmountActual)
                .HasPrecision(18, 2)
                .HasComment("ยอดที่ทำได้");
            entity.Property(e => e.AmountTarget)
                .HasPrecision(18, 2)
                .HasComment("ยอดเป้าหมาย");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasComment("พนักงาน")
                .HasColumnType("int(11)");
            entity.Property(e => e.Year).HasColumnType("int(11)");

            entity.HasOne(d => d.CreateByNavigation).WithMany(p => p.User_Target_SaleCreateByNavigations)
                .HasForeignKey(d => d.CreateBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_target_sale_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.User_Target_SaleUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_target_sale_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
