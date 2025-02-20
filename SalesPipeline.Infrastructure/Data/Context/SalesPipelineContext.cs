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

    public virtual DbSet<Assignment_Center> Assignment_Centers { get; set; }

    public virtual DbSet<Assignment_RM> Assignment_RMs { get; set; }

    public virtual DbSet<Assignment_RM_Sale> Assignment_RM_Sales { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customer_Committee> Customer_Committees { get; set; }

    public virtual DbSet<Customer_History> Customer_Histories { get; set; }

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

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<Loan_AppLoan> Loan_AppLoans { get; set; }

    public virtual DbSet<Loan_BusType> Loan_BusTypes { get; set; }

    public virtual DbSet<Loan_Period> Loan_Periods { get; set; }

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

    public virtual DbSet<Master_Pre_Applicant_Loan> Master_Pre_Applicant_Loans { get; set; }

    public virtual DbSet<Master_Pre_BusinessType> Master_Pre_BusinessTypes { get; set; }

    public virtual DbSet<Master_Pre_Interest_PayType> Master_Pre_Interest_PayTypes { get; set; }

    public virtual DbSet<Master_Pre_Interest_RateType> Master_Pre_Interest_RateTypes { get; set; }

    public virtual DbSet<Master_Proceed> Master_Proceeds { get; set; }

    public virtual DbSet<Master_ProductProgramBank> Master_ProductProgramBanks { get; set; }

    public virtual DbSet<Master_ReasonReturn> Master_ReasonReturns { get; set; }

    public virtual DbSet<Master_Reason_CloseSale> Master_Reason_CloseSales { get; set; }

    public virtual DbSet<Master_StatusSale> Master_StatusSales { get; set; }

    public virtual DbSet<Master_TSIC> Master_TSICs { get; set; }

    public virtual DbSet<Master_TypeLoanRequest> Master_TypeLoanRequests { get; set; }

    public virtual DbSet<Master_Year> Master_Years { get; set; }

    public virtual DbSet<Master_Yield> Master_Yields { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Pre_Cal> Pre_Cals { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_App> Pre_Cal_Fetu_Apps { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_App_Item> Pre_Cal_Fetu_App_Items { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_App_Item_Score> Pre_Cal_Fetu_App_Item_Scores { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_Bu> Pre_Cal_Fetu_Bus { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_Bus_Item> Pre_Cal_Fetu_Bus_Items { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_Bus_Item_Score> Pre_Cal_Fetu_Bus_Item_Scores { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_Stan> Pre_Cal_Fetu_Stans { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_Stan_ItemOption> Pre_Cal_Fetu_Stan_ItemOptions { get; set; }

    public virtual DbSet<Pre_Cal_Fetu_Stan_Score> Pre_Cal_Fetu_Stan_Scores { get; set; }

    public virtual DbSet<Pre_Cal_Info> Pre_Cal_Infos { get; set; }

    public virtual DbSet<Pre_Cal_Info_Score> Pre_Cal_Info_Scores { get; set; }

    public virtual DbSet<Pre_Cal_WeightFactor> Pre_Cal_WeightFactors { get; set; }

    public virtual DbSet<Pre_Cal_WeightFactor_Item> Pre_Cal_WeightFactor_Items { get; set; }

    public virtual DbSet<Pre_ChancePass> Pre_ChancePasses { get; set; }

    public virtual DbSet<Pre_CreditScore> Pre_CreditScores { get; set; }

    public virtual DbSet<Pre_Factor> Pre_Factors { get; set; }

    public virtual DbSet<Pre_Factor_App> Pre_Factor_Apps { get; set; }

    public virtual DbSet<Pre_Factor_Bu> Pre_Factor_Bus { get; set; }

    public virtual DbSet<Pre_Factor_Info> Pre_Factor_Infos { get; set; }

    public virtual DbSet<Pre_Factor_Stan> Pre_Factor_Stans { get; set; }

    public virtual DbSet<Pre_Result> Pre_Results { get; set; }

    public virtual DbSet<Pre_Result_Item> Pre_Result_Items { get; set; }

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

    public virtual DbSet<Sale_Document_Upload> Sale_Document_Uploads { get; set; }

    public virtual DbSet<Sale_Duration> Sale_Durations { get; set; }

    public virtual DbSet<Sale_Meet> Sale_Meets { get; set; }

    public virtual DbSet<Sale_Partner> Sale_Partners { get; set; }

    public virtual DbSet<Sale_Phoenix> Sale_Phoenixes { get; set; }

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

    public virtual DbSet<System_Config> System_Configs { get; set; }

    public virtual DbSet<System_SLA> System_SLAs { get; set; }

    public virtual DbSet<System_Signature> System_Signatures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<User_Area> User_Areas { get; set; }

    public virtual DbSet<User_Level> User_Levels { get; set; }

    public virtual DbSet<User_Login_Log> User_Login_Logs { get; set; }

    public virtual DbSet<User_Login_TokenNoti> User_Login_TokenNotis { get; set; }

    public virtual DbSet<User_Permission> User_Permissions { get; set; }

    public virtual DbSet<User_RefreshToken> User_RefreshTokens { get; set; }

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

            entity.HasIndex(e => e.Master_Branch_RegionId, "Master_Branch_RegionId");

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
            entity.Property(e => e.Master_Branch_RegionId).HasComment("กิจการสาขาภาค");
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

            entity.HasOne(d => d.Master_Branch_Region).WithMany(p => p.Assignment_BranchRegs)
                .HasForeignKey(d => d.Master_Branch_RegionId)
                .HasConstraintName("assignment_branchreg_ibfk_3");

            entity.HasOne(d => d.User).WithMany(p => p.Assignment_BranchRegs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_branchreg_ibfk_1");
        });

        modelBuilder.Entity<Assignment_Center>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Assignment_Center");

            entity.HasIndex(e => e.UserId, "UserId");

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

            entity.HasOne(d => d.User).WithMany(p => p.Assignment_Centers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("assignment_center_ibfk_1");
        });

        modelBuilder.Entity<Assignment_RM>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Assignment_RM", tb => tb.HasComment("พนักงานที่ถูกมอบหมาย"));

            entity.HasIndex(e => e.Status, "Status");

            entity.HasIndex(e => e.UserId, "UserId");

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
            entity.Property(e => e.BranchId)
                .HasComment("สาขา")
                .HasColumnType("int(11)");
            entity.Property(e => e.BranchName)
                .HasMaxLength(255)
                .HasComment("ชื่อสาขา");
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
            entity.Property(e => e.ContactProvinceId)
                .HasComment("จังหวัดผู้ติดต่อ")
                .HasColumnType("int(11)");
            entity.Property(e => e.ContactProvinceName).HasMaxLength(255);
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

        modelBuilder.Entity<Customer_History>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Customer_History");

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateByFullName).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
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
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ProvinceID).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
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

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Loan", tb => tb.HasComment("สินเชื่อ"));

            entity.HasIndex(e => e.Master_Pre_Interest_PayTypeId, "Master_Pre_Interest_PayTypeId");

            entity.Property(e => e.Condition)
                .HasMaxLength(500)
                .HasComment("เงื่อนไข");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Master_Pre_Interest_PayTypeId).HasComment("ประเภทการชำระดอกเบี้ย");
            entity.Property(e => e.Master_Pre_Interest_PayTypeName).HasMaxLength(255);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อสินเชื่อ");
            entity.Property(e => e.PeriodNumber)
                .HasComment("จำนวนช่วงเวลา")
                .HasColumnType("int(11)");
            entity.Property(e => e.RiskPremiumYear)
                .HasPrecision(18, 3)
                .HasComment("Risk Premium รายปี");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Master_Pre_Interest_PayType).WithMany(p => p.Loans)
                .HasForeignKey(d => d.Master_Pre_Interest_PayTypeId)
                .HasConstraintName("loan_ibfk_1");
        });

        modelBuilder.Entity<Loan_AppLoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Loan_AppLoan", tb => tb.HasComment("ประเภทผู้ขอในระยะที่"));

            entity.HasIndex(e => e.LoanId, "LoanId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Master_Pre_Applicant_LoanName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Loan).WithMany(p => p.Loan_AppLoans)
                .HasForeignKey(d => d.LoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loan_apploan_ibfk_1");
        });

        modelBuilder.Entity<Loan_BusType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Loan_BusType", tb => tb.HasComment("ประเภทธุรกิจในระยะที่"));

            entity.HasIndex(e => e.LoanId, "LoanId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Master_Pre_BusinessTypeName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Loan).WithMany(p => p.Loan_BusTypes)
                .HasForeignKey(d => d.LoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loan_bustype_ibfk_1");
        });

        modelBuilder.Entity<Loan_Period>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Loan_Period", tb => tb.HasComment("รายละเอียดงวดสินเชื่อ"));

            entity.HasIndex(e => e.LoanId, "LoanId");

            entity.HasIndex(e => e.Master_Pre_Interest_RateTypeId, "Master_Pre_Interest_RateTypeId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LoanId).HasComment("FK สินเชื่อ");
            entity.Property(e => e.Master_Pre_Interest_RateTypeCode).HasMaxLength(255);
            entity.Property(e => e.Master_Pre_Interest_RateTypeId).HasComment("FK ประเภทอัตราดอกเบี้ย");
            entity.Property(e => e.Master_Pre_Interest_RateTypeName).HasMaxLength(255);
            entity.Property(e => e.PeriodNo)
                .HasComment("ระยะที่")
                .HasColumnType("int(11)");
            entity.Property(e => e.RateValue)
                .HasPrecision(18, 3)
                .HasComment("อัตราดอกเบี้ย %");
            entity.Property(e => e.RateValueOriginal)
                .HasPrecision(18, 3)
                .HasComment("อัตราดอกเบี้ยตาม master");
            entity.Property(e => e.SpecialRate)
                .HasPrecision(18, 2)
                .HasComment("ค่าเพิ่มลบดอกเบี้ย %");
            entity.Property(e => e.SpecialType)
                .HasComment("1=เพิ่ม 2=ลบ")
                .HasColumnType("int(11)");
            entity.Property(e => e.StartYear)
                .HasComment("เริ่มปีที่")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Loan).WithMany(p => p.Loan_Periods)
                .HasForeignKey(d => d.LoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loan_period_ibfk_1");

            entity.HasOne(d => d.Master_Pre_Interest_RateType).WithMany(p => p.Loan_Periods)
                .HasForeignKey(d => d.Master_Pre_Interest_RateTypeId)
                .HasConstraintName("loan_period_ibfk_2");
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
            entity.Property(e => e.GroupMaster_BusinessTypeId).HasMaxLength(500);
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

        modelBuilder.Entity<Master_Pre_Applicant_Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Pre_Applicant_Loan", tb => tb.HasComment("ประเภทผู้ขอสินเชื่อ"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Pre_BusinessType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Pre_BusinessType", tb => tb.HasComment("ประเภทธุรกิจ pre approve"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Pre_Interest_PayType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Pre_Interest_PayType", tb => tb.HasComment("ประเภทการชำระดอกเบี้ย"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Pre_Interest_RateType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Pre_Interest_RateType", tb => tb.HasComment("ประเภทอัตราดอกเบี้ย"));

            entity.Property(e => e.Code).HasMaxLength(255);
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อเต็ม");
            entity.Property(e => e.Rate)
                .HasPrecision(18, 3)
                .HasComment("อัตราดอกเบี้ย");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
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
            entity.Property(e => e.NameFCC).HasMaxLength(255);
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
            entity.Property(e => e.EventName).HasMaxLength(255);
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

        modelBuilder.Entity<Pre_Cal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal");

            entity.HasIndex(e => e.Master_Pre_Applicant_LoanId, "Master_Pre_Applicant_LoanId");

            entity.HasIndex(e => e.Master_Pre_BusinessTypeId, "Master_Pre_BusinessTypeId");

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DisplayResultType)
                .HasComment("1=แสดงเฉพาะคะแนนรวม")
                .HasColumnType("int(11)");
            entity.Property(e => e.Master_Pre_Applicant_LoanId).HasComment("ประเภทผู้ขอสินเชื่อ");
            entity.Property(e => e.Master_Pre_Applicant_LoanName).HasMaxLength(255);
            entity.Property(e => e.Master_Pre_BusinessTypeId).HasComment("ประเภทธุรกิจ");
            entity.Property(e => e.Master_Pre_BusinessTypeName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Master_Pre_Applicant_Loan).WithMany(p => p.Pre_Cals)
                .HasForeignKey(d => d.Master_Pre_Applicant_LoanId)
                .HasConstraintName("pre_cal_ibfk_1");

            entity.HasOne(d => d.Master_Pre_BusinessType).WithMany(p => p.Pre_Cals)
                .HasForeignKey(d => d.Master_Pre_BusinessTypeId)
                .HasConstraintName("pre_cal_ibfk_2");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_App>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Fetu_App", tb => tb.HasComment("ตัวแปรคำนวณ คุณสมบัติตามประเภทผู้ขอ"));

            entity.HasIndex(e => e.Pre_CalId, "Pre_CalId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.HighScore)
                .HasComment("คะแนนสูงสุด")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal).WithMany(p => p.Pre_Cal_Fetu_Apps)
                .HasForeignKey(d => d.Pre_CalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_app_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_App_Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Fetu_App_Item");

            entity.HasIndex(e => e.Pre_Cal_Fetu_AppId, "Pre_Cal_Fetu_AppId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal_Fetu_App).WithMany(p => p.Pre_Cal_Fetu_App_Items)
                .HasForeignKey(d => d.Pre_Cal_Fetu_AppId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_app_item_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_App_Item_Score>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Fetu_App_Item_Score");

            entity.HasIndex(e => e.Pre_Cal_Fetu_App_ItemId, "Pre_Cal_Fetu_App_ItemId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Score)
                .HasPrecision(18, 4)
                .HasComment("คะแนน");
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal_Fetu_App_Item).WithMany(p => p.Pre_Cal_Fetu_App_Item_Scores)
                .HasForeignKey(d => d.Pre_Cal_Fetu_App_ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_app_item_score_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_Bu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("ตัวแปรคำนวณ คุณสมบัติตามประเภทธุรกิจ"));

            entity.HasIndex(e => e.Pre_CalId, "Pre_CalId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.HighScore)
                .HasComment("คะแนนสูงสุด")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal).WithMany(p => p.Pre_Cal_Fetu_Bus)
                .HasForeignKey(d => d.Pre_CalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_bus_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_Bus_Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Fetu_Bus_Item");

            entity.HasIndex(e => e.Pre_Cal_Fetu_BusId, "Pre_Cal_Fetu_BusId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal_Fetu_Bus).WithMany(p => p.Pre_Cal_Fetu_Bus_Items)
                .HasForeignKey(d => d.Pre_Cal_Fetu_BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_bus_item_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_Bus_Item_Score>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Fetu_Bus_Item_Score");

            entity.HasIndex(e => e.Pre_Cal_Fetu_Bus_ItemId, "Pre_Cal_Fetu_Bus_ItemId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Score)
                .HasPrecision(18, 4)
                .HasComment("คะแนน");
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal_Fetu_Bus_Item).WithMany(p => p.Pre_Cal_Fetu_Bus_Item_Scores)
                .HasForeignKey(d => d.Pre_Cal_Fetu_Bus_ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_bus_item_score_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_Stan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Fetu_Stan", tb => tb.HasComment("ตัวแปรคำนวณ คุณสมบัติมาตรฐาน"));

            entity.HasIndex(e => e.Pre_CalId, "Pre_CalId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.HighScore)
                .HasComment("คะแนนสูงสุด")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal).WithMany(p => p.Pre_Cal_Fetu_Stans)
                .HasForeignKey(d => d.Pre_CalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_stan_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_Stan_ItemOption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Fetu_Stan_ItemOption");

            entity.HasIndex(e => e.Pre_Cal_Fetu_StanId, "Pre_Cal_InfoId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Type)
                .HasComment("1=ประเภทหลักประกัน 2=ประวัติการชำระหนี้")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Pre_Cal_Fetu_Stan).WithMany(p => p.Pre_Cal_Fetu_Stan_ItemOptions)
                .HasForeignKey(d => d.Pre_Cal_Fetu_StanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_stan_itemoption_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Fetu_Stan_Score>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Fetu_Stan_Score");

            entity.HasIndex(e => e.Pre_Cal_Fetu_StanId, "Pre_Cal_InfoId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Score)
                .HasPrecision(18, 4)
                .HasComment("คะแนน");
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Type)
                .HasComment("1=น้ำหนักของแต่ละปัจจัยรายได้ต่อรายจ่าย\r\n2=น้ำหนักของแต่ละปัจจัยหลักประกันมูลค่าหนี้\r\n3=น้ำหนักของแต่ละปัจจัยหนี้สินต่อรายได้อื่นๆ\r\n4=ปริมาณเงินฝาก\r\n5=ประเภทหลักประกัน\r\n6=มูลค่าสินเชื่อ\r\n7=ประวัติการชำระหนี้")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Pre_Cal_Fetu_Stan).WithMany(p => p.Pre_Cal_Fetu_Stan_Scores)
                .HasForeignKey(d => d.Pre_Cal_Fetu_StanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_fetu_stan_score_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Info>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Info", tb => tb.HasComment("ตัวแปรคำนวณ ข้อมูลการขอสินเชื่อ"));

            entity.HasIndex(e => e.Pre_CalId, "Pre_CalId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.HighScore)
                .HasComment("คะแนนสูงสุด")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal).WithMany(p => p.Pre_Cal_Infos)
                .HasForeignKey(d => d.Pre_CalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_info_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_Info_Score>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_Info_Score");

            entity.HasIndex(e => e.Pre_Cal_InfoId, "Pre_Cal_InfoId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasColumnType("int(11)");
            entity.Property(e => e.Score)
                .HasPrecision(18, 4)
                .HasComment("คะแนน");
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal_Info).WithMany(p => p.Pre_Cal_Info_Scores)
                .HasForeignKey(d => d.Pre_Cal_InfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_info_score_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_WeightFactor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_WeightFactor", tb => tb.HasComment("น้ำหนักของแต่ละปัจจัย"));

            entity.HasIndex(e => e.Pre_CalId, "Pre_CalId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.TotalPercent).HasPrecision(18, 4);
            entity.Property(e => e.Type)
                .HasComment("1=ข้อมูลการขอสินเชื่อ\r\n2=คุณสมบัติมารตฐาน\r\n3=คุณสมบัติตามประเภทผู้ขอ\r\n4=คุณสมบัติตามประเภทธุรกิจ")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal).WithMany(p => p.Pre_Cal_WeightFactors)
                .HasForeignKey(d => d.Pre_CalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_weightfactor_ibfk_1");
        });

        modelBuilder.Entity<Pre_Cal_WeightFactor_Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Cal_WeightFactor_Item");

            entity.HasIndex(e => e.Pre_Cal_WeightFactorId, "Pre_Cal_WeightFactorId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Percent).HasPrecision(18, 4);
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.StanScoreType)
                .HasComment("ประเภท คะแนนคุณสมบัติมาตรฐาน")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal_WeightFactor).WithMany(p => p.Pre_Cal_WeightFactor_Items)
                .HasForeignKey(d => d.Pre_Cal_WeightFactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_cal_weightfactor_item_ibfk_1");
        });

        modelBuilder.Entity<Pre_ChancePass>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_ChancePass", tb => tb.HasComment("โอกาสขอสินเชื่อผ่าน"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreditScore).HasMaxLength(255);
            entity.Property(e => e.Prob).HasMaxLength(255);
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Z).HasMaxLength(255);
        });

        modelBuilder.Entity<Pre_CreditScore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_CreditScore", tb => tb.HasComment("Credit Score"));

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreditScore).HasColumnType("int(11)");
            entity.Property(e => e.CreditScoreColor).HasMaxLength(255);
            entity.Property(e => e.Grade).HasMaxLength(255);
            entity.Property(e => e.Level).HasMaxLength(255);
            entity.Property(e => e.LimitMultiplier).HasMaxLength(255);
            entity.Property(e => e.RateMultiplier).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Pre_Factor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Factor");

            entity.HasIndex(e => e.Pre_CalId, "Pre_CalId");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasComment("ชื่อบริษัท");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Cal).WithMany(p => p.Pre_Factors)
                .HasForeignKey(d => d.Pre_CalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_factor_ibfk_2");

            entity.HasOne(d => d.Sale).WithMany(p => p.Pre_Factors)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_factor_ibfk_1");
        });

        modelBuilder.Entity<Pre_Factor_App>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Factor_App");

            entity.HasIndex(e => e.Pre_FactorId, "Pre_FactorId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Pre_Cal_Fetu_App_ItemName).HasMaxLength(255);
            entity.Property(e => e.Pre_Cal_Fetu_App_Item_ScoreName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Factor).WithMany(p => p.Pre_Factor_Apps)
                .HasForeignKey(d => d.Pre_FactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_factor_app_ibfk_1");
        });

        modelBuilder.Entity<Pre_Factor_Bu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.Pre_FactorId, "Pre_FactorId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Pre_Cal_Fetu_Bus_ItemName).HasMaxLength(255);
            entity.Property(e => e.Pre_Cal_Fetu_Bus_Item_ScoreName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Factor).WithMany(p => p.Pre_Factor_Bus)
                .HasForeignKey(d => d.Pre_FactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_factor_bus_ibfk_1");
        });

        modelBuilder.Entity<Pre_Factor_Info>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Factor_Info");

            entity.HasIndex(e => e.LoanId, "LoanId");

            entity.HasIndex(e => e.Master_Pre_Applicant_LoanId, "Master_Pre_Applicant_LoanId");

            entity.HasIndex(e => e.Master_Pre_BusinessTypeId, "Master_Pre_BusinessTypeId");

            entity.HasIndex(e => e.Pre_FactorId, "Pre_FactorId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.InstallmentPayYear)
                .HasComment("จำนวนงวดชำระต่อปี")
                .HasColumnType("int(11)");
            entity.Property(e => e.LoanIName).HasMaxLength(255);
            entity.Property(e => e.LoanPeriod)
                .HasComment("ระยะเวลาสินเชื่อ")
                .HasColumnType("int(11)");
            entity.Property(e => e.LoanValue)
                .HasPrecision(18, 2)
                .HasComment("มูลค่าสินเชื่อ");
            entity.Property(e => e.Master_Pre_Applicant_LoanId).HasComment("ประเภทผู้ขอสินเชื่อ");
            entity.Property(e => e.Master_Pre_Applicant_LoanName).HasMaxLength(255);
            entity.Property(e => e.Master_Pre_BusinessTypeId).HasComment("ประเภทธุรกิจ");
            entity.Property(e => e.Master_Pre_BusinessTypeName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Loan).WithMany(p => p.Pre_Factor_Infos)
                .HasForeignKey(d => d.LoanId)
                .HasConstraintName("pre_factor_info_ibfk_2");

            entity.HasOne(d => d.Master_Pre_Applicant_Loan).WithMany(p => p.Pre_Factor_Infos)
                .HasForeignKey(d => d.Master_Pre_Applicant_LoanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_factor_info_ibfk_3");

            entity.HasOne(d => d.Master_Pre_BusinessType).WithMany(p => p.Pre_Factor_Infos)
                .HasForeignKey(d => d.Master_Pre_BusinessTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_factor_info_ibfk_4");

            entity.HasOne(d => d.Pre_Factor).WithMany(p => p.Pre_Factor_Infos)
                .HasForeignKey(d => d.Pre_FactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_factor_info_ibfk_1");
        });

        modelBuilder.Entity<Pre_Factor_Stan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Factor_Stan");

            entity.HasIndex(e => e.Pre_FactorId, "Pre_FactorId");

            entity.HasIndex(e => e.Stan_ItemOptionId_Type1, "Stan_ItemOptionId_Type1");

            entity.HasIndex(e => e.Stan_ItemOptionId_Type2, "Stan_ItemOptionId_Type2");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DepositBAAC)
                .HasPrecision(18, 2)
                .HasComment("ปริมาณเงินฝากกับ ธกส.");
            entity.Property(e => e.Expenses)
                .HasPrecision(18, 2)
                .HasComment("รายจ่าย");
            entity.Property(e => e.Income)
                .HasPrecision(18, 2)
                .HasComment("รายได้");
            entity.Property(e => e.IncomeDebtPeriod)
                .HasPrecision(18, 2)
                .HasComment("รายได้ที่ได้ตามระยะงวดหนี้สินด้านบน");
            entity.Property(e => e.OtherDebts)
                .HasPrecision(18, 2)
                .HasComment("หนี้สินอื่นๆ");
            entity.Property(e => e.Stan_ItemOptionId_Type1).HasComment("ประเภทหลักประกัน");
            entity.Property(e => e.Stan_ItemOptionId_Type2).HasComment("ประวัติการชำระหนี้");
            entity.Property(e => e.Stan_ItemOptionName_Type1).HasMaxLength(255);
            entity.Property(e => e.Stan_ItemOptionName_Type2).HasMaxLength(255);
            entity.Property(e => e.Stan_ItemOptionValue_Type1)
                .HasPrecision(18, 2)
                .HasComment("มูลค่าหลักประกัน");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.Pre_Factor).WithMany(p => p.Pre_Factor_Stans)
                .HasForeignKey(d => d.Pre_FactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_factor_stan_ibfk_1");
        });

        modelBuilder.Entity<Pre_Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Result");

            entity.HasIndex(e => e.Pre_FactorId, "Pre_FactorId");

            entity.Property(e => e.Ch_CreditScore).HasMaxLength(255);
            entity.Property(e => e.Ch_Prob).HasMaxLength(255);
            entity.Property(e => e.Ch_Z).HasMaxLength(255);
            entity.Property(e => e.ChancePercent)
                .HasMaxLength(255)
                .HasComment("โอกาสขอสินเชื่อผ่าน");
            entity.Property(e => e.Cr_CreditScore).HasColumnType("int(255)");
            entity.Property(e => e.Cr_Grade).HasMaxLength(255);
            entity.Property(e => e.Cr_Level).HasMaxLength(255);
            entity.Property(e => e.Cr_LimitMultiplier).HasMaxLength(255);
            entity.Property(e => e.Cr_RateMultiplier).HasMaxLength(255);
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DisplayResultType)
                .HasComment("1=แสดงเฉพาะคะแนนรวม")
                .HasColumnType("int(11)");
            entity.Property(e => e.IncomeTotal)
                .HasPrecision(18, 2)
                .HasComment("รายได้ทั้งหมด");
            entity.Property(e => e.InstallmentAll)
                .HasPrecision(18, 2)
                .HasComment("ผ่อนทั้งหมด");
            entity.Property(e => e.PresSave)
                .HasComment("1=มีการกดบันทึก")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.RatioInstallmentIncome)
                .HasPrecision(18, 2)
                .HasComment("อัตราส่วนผ่อน/รายได้");
            entity.Property(e => e.ResultLoan)
                .HasMaxLength(255)
                .HasComment("ผลการขอสินเชื่อ");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.TotalScore)
                .HasPrecision(18, 2)
                .HasComment("คะแนนรวม");

            entity.HasOne(d => d.Pre_Factor).WithMany(p => p.Pre_Results)
                .HasForeignKey(d => d.Pre_FactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_result_ibfk_1");
        });

        modelBuilder.Entity<Pre_Result_Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Pre_Result_Item");

            entity.HasIndex(e => e.Pre_ResultId, "Pre_ResultId");

            entity.Property(e => e.AnalysisFactor)
                .HasMaxLength(255)
                .HasComment("ปัจจัยการวิเคราะห์");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Feature)
                .HasMaxLength(255)
                .HasComment("คุณสมบัติ");
            entity.Property(e => e.Ratio)
                .HasPrecision(18, 2)
                .HasComment("อัตราส่วน");
            entity.Property(e => e.Score)
                .HasPrecision(18, 2)
                .HasComment("คะแนน");
            entity.Property(e => e.ScoreResult)
                .HasPrecision(18, 2)
                .HasComment("ผลคะแนน");
            entity.Property(e => e.SequenceNo)
                .HasComment("ลำดับ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Type)
                .HasComment("1=ข้อมูลการขอสินเชื่อ\r\n2=คุณสมบัติมารตฐาน\r\n3=คุณสมบัติตามประเภทผู้ขอ\r\n4=คุณสมบัติตามประเภทธุรกิจ")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Pre_Result).WithMany(p => p.Pre_Result_Items)
                .HasForeignKey(d => d.Pre_ResultId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pre_result_item_ibfk_1");
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

            entity.Property(e => e.AssCenterAlready).HasComment("ผู้จัดการศูนย์ได้รับมอบหมายแล้ว");
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
            entity.Property(e => e.AssUserAlready).HasComment("ได้รับมอบหมายแล้ว");
            entity.Property(e => e.AssUserDate)
                .HasComment("วันที่มอบหมาย")
                .HasColumnType("datetime");
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
            entity.Property(e => e.IsRePurpose).HasComment("สร้างโดยกด Re-Purpose");
            entity.Property(e => e.LoanAmount)
                .HasPrecision(18, 2)
                .HasComment("จำนวนการกู้");
            entity.Property(e => e.LoanPeriod)
                .HasComment("ระยะเวลาสินเชื่อ")
                .HasColumnType("int(11)");
            entity.Property(e => e.Master_Branch_RegionId).HasComment("กิจการสาขาภาค");
            entity.Property(e => e.Master_Branch_RegionName).HasMaxLength(255);
            entity.Property(e => e.Master_Reason_CloseSaleId).HasComment("เหตุผลไม่ประสงค์กู้");
            entity.Property(e => e.PercentChanceLoanPass).HasComment("เปอร์เซ็นโอกาสกู้ผ่าน");
            entity.Property(e => e.PercentChanceLoanPassName)
                .HasMaxLength(255)
                .HasComment("เปอร์เซ็นโอกาสกู้ผ่าน");
            entity.Property(e => e.ProjectLoanName)
                .HasMaxLength(255)
                .HasComment("ชื่อโครงการสินเชื่อ");
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

            entity.Property(e => e.AppointmentDate)
                .HasComment("วันที่นัดหมาย")
                .HasColumnType("datetime");
            entity.Property(e => e.AppointmentTime)
                .HasComment("เวลาที่นัดหมาย")
                .HasColumnType("time");
            entity.Property(e => e.ContactDate)
                .HasComment("วันที่ติดต่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DesireLoanId)
                .HasComment("1=ประสงค์กู้ 2=ไม่ประสงค์กู้")
                .HasColumnType("int(11)");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasComment("สถานที่");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.NextActionId)
                .HasComment("1=ปิดการขาย 2=ติดต่ออีกครั้ง")
                .HasColumnType("int(11)");
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
            entity.Property(e => e.AttachmentPath)
                .HasMaxLength(255)
                .HasComment("ไฟล์แนบ");
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
            entity.Property(e => e.IsScheduledJob)
                .HasComment("1=กำหนดเวลาแล้ว")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.IsScheduledJobSucceed)
                .HasComment("1=แจ้งเตือนแล้ว")
                .HasColumnType("smallint(6)");
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
            entity.Property(e => e.PercentChanceLoanPass).HasComment("เปอร์เซ็นโอกาสกู้ผ่าน");
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
            entity.Property(e => e.Createdfrom)
                .HasComment("1=ฟอร์มเพิ่มลูกค้า backend")
                .HasColumnType("smallint(6)");
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

            entity.HasIndex(e => e.Master_ProductProgramBankId, "Master_ProductProgramBankId");

            entity.HasIndex(e => e.Master_TypeLoanRequestId, "Master_TypeLoanRequest");

            entity.HasIndex(e => e.SaleId, "SaleId");

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
            entity.Property(e => e.Master_BusinessTypeName).HasMaxLength(255);
            entity.Property(e => e.Master_ProductProgramBankName).HasMaxLength(255);
            entity.Property(e => e.Master_TypeLoanRequesName).HasMaxLength(255);
            entity.Property(e => e.Master_TypeLoanRequestSpecify)
                .HasMaxLength(255)
                .HasComment("ระบุ");
            entity.Property(e => e.NameEn)
                .HasMaxLength(255)
                .HasComment("ชื่อภาษาอังกฤษ");
            entity.Property(e => e.NameTh)
                .HasMaxLength(255)
                .HasComment("ชื่อภาษาไทย");
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
            entity.Property(e => e.SignatureEmployeeLoanDate)
                .HasComment("วันที่เซ็นพนักงานสินเชื่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.SignatureEmployeeLoanPath)
                .HasMaxLength(255)
                .HasComment("รูปลายเซ็นพนักงานสินเชื่อ");
            entity.Property(e => e.SignatureMCenterDate)
                .HasComment("วันที่เซ็นผู้จัดการศูนย์")
                .HasColumnType("datetime");
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

            entity.HasOne(d => d.Master_ProductProgramBank).WithMany(p => p.Sale_Documents)
                .HasForeignKey(d => d.Master_ProductProgramBankId)
                .HasConstraintName("sale_document_ibfk_3");

            entity.HasOne(d => d.Master_TypeLoanRequest).WithMany(p => p.Sale_Documents)
                .HasForeignKey(d => d.Master_TypeLoanRequestId)
                .HasConstraintName("sale_document_ibfk_2");

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Documents)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_document_ibfk_1");
        });

        modelBuilder.Entity<Sale_Document_Upload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Document_Upload");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasComment("ชื่อไฟล์ที่ใช้ในระบบ");
            entity.Property(e => e.FileSize)
                .HasComment("ขนาดไฟล์")
                .HasColumnType("bigint(20)");
            entity.Property(e => e.MimeType)
                .HasMaxLength(50)
                .HasComment("นามสกุลไฟล์");
            entity.Property(e => e.OriginalFileName)
                .HasMaxLength(255)
                .HasComment("ชื่อเดิมไฟล์");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Type)
                .HasComment("1=รูปบัตรประชาชน \r\n2=ทะเบียนนบ้าน \r\n3=เอกสารอื่นๆ \r\n4=ลายเซ็นผู้กู้ยืม \r\n5=ลายเซ็นพนักงานสินเชื่อ \r\n6=เอกสารเพิ่มเติม")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Url).HasMaxLength(500);

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Document_Uploads)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_document_upload_ibfk_1");
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

        modelBuilder.Entity<Sale_Phoenix>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale_Phoenix");

            entity.HasIndex(e => e.SaleId, "SaleId");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.ana_no).HasMaxLength(255);
            entity.Property(e => e.app_no).HasMaxLength(255);
            entity.Property(e => e.approve_by).HasMaxLength(255);
            entity.Property(e => e.approve_date).HasMaxLength(255);
            entity.Property(e => e.approve_level).HasMaxLength(255);
            entity.Property(e => e.branch_customer).HasMaxLength(255);
            entity.Property(e => e.branch_user).HasMaxLength(255);
            entity.Property(e => e.cif_name).HasMaxLength(255);
            entity.Property(e => e.cif_no).HasMaxLength(255);
            entity.Property(e => e.create_by).HasMaxLength(255);
            entity.Property(e => e.created_date).HasMaxLength(255);
            entity.Property(e => e.fin_type).HasMaxLength(255);
            entity.Property(e => e.status_code).HasMaxLength(255);
            entity.Property(e => e.status_type).HasMaxLength(255);
            entity.Property(e => e.update_by).HasMaxLength(255);
            entity.Property(e => e.update_date).HasMaxLength(255);
            entity.Property(e => e.workflow_id).HasMaxLength(255);

            entity.HasOne(d => d.Sale).WithMany(p => p.Sale_Phoenixes)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sale_phoenix_ibfk_1");
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
                .HasComment("1=แจ้งข้อมูลเพิ่มเติม 2=ติดต่อขอเอกสาร 3=เข้าพบรับเอกสาร 4=ไม่ผ่านการพิจารณา 5=รอปิดการขาย")
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
            entity.Property(e => e.Master_BusinessTypeId).HasComment("ประเภทกิจการ");
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

        modelBuilder.Entity<System_Config>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("System_Config");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Group).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.Value).HasMaxLength(255);
        });

        modelBuilder.Entity<System_SLA>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("System_SLA", tb => tb.HasComment("SLA การติดต่อ"));

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Number).HasColumnType("int(11)");
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
            entity.Property(e => e.Create_Type)
                .HasComment("1=iAuthen")
                .HasColumnType("int(11)");
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
            entity.Property(e => e.TokenApi).HasMaxLength(255);
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UrlSignature)
                .HasMaxLength(300)
                .HasComment("url ลายเซ็น");
            entity.Property(e => e.UserName).HasMaxLength(255);
            entity.Property(e => e.authen_fail_time).HasColumnType("int(11)");
            entity.Property(e => e.branch_code).HasMaxLength(255);
            entity.Property(e => e.branch_name).HasMaxLength(255);
            entity.Property(e => e.cbs_id).HasMaxLength(255);
            entity.Property(e => e.change_password_url).HasMaxLength(255);
            entity.Property(e => e.create_password_url).HasMaxLength(255);
            entity.Property(e => e.email_baac).HasMaxLength(255);
            entity.Property(e => e.employee_id).HasMaxLength(255);
            entity.Property(e => e.employee_position_id).HasMaxLength(255);
            entity.Property(e => e.employee_position_level).HasMaxLength(255);
            entity.Property(e => e.employee_position_name).HasMaxLength(255);
            entity.Property(e => e.employee_status).HasMaxLength(255);
            entity.Property(e => e.first_name_th).HasMaxLength(255);
            entity.Property(e => e.job_field_id).HasMaxLength(255);
            entity.Property(e => e.job_field_name).HasMaxLength(255);
            entity.Property(e => e.job_id).HasMaxLength(255);
            entity.Property(e => e.job_name).HasMaxLength(255);
            entity.Property(e => e.last_name_th).HasMaxLength(255);
            entity.Property(e => e.lastauthen_timestamp).HasColumnType("datetime");
            entity.Property(e => e.mobile_no).HasMaxLength(255);
            entity.Property(e => e.name_en).HasMaxLength(255);
            entity.Property(e => e.org_id).HasMaxLength(255);
            entity.Property(e => e.org_name).HasMaxLength(255);
            entity.Property(e => e.organization_48).HasMaxLength(255);
            entity.Property(e => e.organization_abbreviation).HasMaxLength(255);
            entity.Property(e => e.organization_upper_id).HasMaxLength(255);
            entity.Property(e => e.organization_upper_id2).HasMaxLength(255);
            entity.Property(e => e.organization_upper_id3).HasMaxLength(255);
            entity.Property(e => e.organization_upper_name).HasMaxLength(255);
            entity.Property(e => e.organization_upper_name2).HasMaxLength(255);
            entity.Property(e => e.organization_upper_name3).HasMaxLength(255);
            entity.Property(e => e.timeresive).HasColumnType("datetime");
            entity.Property(e => e.timesend).HasColumnType("datetime");
            entity.Property(e => e.title_th).HasMaxLength(255);
            entity.Property(e => e.title_th_2).HasMaxLength(255);
            entity.Property(e => e.user_class).HasMaxLength(255);
            entity.Property(e => e.working_status).HasMaxLength(255);

            entity.HasOne(d => d.Level).WithMany(p => p.Users)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.Master_Branch_Region).WithMany(p => p.Users)
                .HasForeignKey(d => d.Master_Branch_RegionId)
                .HasConstraintName("user_ibfk_4");

            entity.HasOne(d => d.Position).WithMany(p => p.Users)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("user_ibfk_3");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<User_Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_Area");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.ProvinceId).HasColumnType("int(11)");
            entity.Property(e => e.ProvinceName).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.User_Areas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_area_ibfk_1");
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
            entity.Property(e => e.AppVersion).HasMaxLength(255);
            entity.Property(e => e.CreateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.DeviceId).HasMaxLength(255);
            entity.Property(e => e.DeviceVersion).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IPAddress).HasMaxLength(255);
            entity.Property(e => e.SystemVersion).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnType("int(11)");
            entity.Property(e => e.tokenNoti).HasMaxLength(300);
        });

        modelBuilder.Entity<User_Login_TokenNoti>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_Login_TokenNoti");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.DeviceId).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnType("int(11)");
            entity.Property(e => e.tokenNoti).HasMaxLength(300);
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

        modelBuilder.Entity<User_RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_RefreshToken");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.TokenValue).HasMaxLength(300);
            entity.Property(e => e.UserId).HasColumnType("int(11)");
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
            entity.Property(e => e.IsAssignCenter).HasComment("1=มีสิทธ์มอบหมาย ผจศ.");
            entity.Property(e => e.IsAssignRM).HasComment("1=มีสิทธ์มอบหมาย rm");
            entity.Property(e => e.IsModify).HasComment("อนุญาตให้แก้ไข");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อหน้าที่");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.iAuthenRoleCode).HasMaxLength(50);
            entity.Property(e => e.iAuthenRoleName).HasMaxLength(255);
            entity.Property(e => e.org_id).HasMaxLength(255);
            entity.Property(e => e.org_name).HasMaxLength(255);
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
