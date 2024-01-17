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

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customer_Committee> Customer_Committees { get; set; }

    public virtual DbSet<Customer_Shareholder> Customer_Shareholders { get; set; }

    public virtual DbSet<FileUpload> FileUploads { get; set; }

    public virtual DbSet<Logging> Loggings { get; set; }

    public virtual DbSet<Master_Branch> Master_Branches { get; set; }

    public virtual DbSet<Master_Chain> Master_Chains { get; set; }

    public virtual DbSet<Master_Division_Branch> Master_Division_Branchs { get; set; }

    public virtual DbSet<Master_Division_Loan> Master_Division_Loans { get; set; }

    public virtual DbSet<Master_LoanType> Master_LoanTypes { get; set; }

    public virtual DbSet<Master_Position> Master_Positions { get; set; }

    public virtual DbSet<Master_ReasonReturn> Master_ReasonReturns { get; set; }

    public virtual DbSet<Master_Region> Master_Regions { get; set; }

    public virtual DbSet<Master_SLAOperation> Master_SLAOperations { get; set; }

    public virtual DbSet<Master_Yield> Master_Yields { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<ProcessSale> ProcessSales { get; set; }

    public virtual DbSet<ProcessSale_Reply> ProcessSale_Replies { get; set; }

    public virtual DbSet<ProcessSale_Reply_Section> ProcessSale_Reply_Sections { get; set; }

    public virtual DbSet<ProcessSale_Reply_Section_Item> ProcessSale_Reply_Section_Items { get; set; }

    public virtual DbSet<ProcessSale_Reply_Section_ItemValue> ProcessSale_Reply_Section_ItemValues { get; set; }

    public virtual DbSet<ProcessSale_Section> ProcessSale_Sections { get; set; }

    public virtual DbSet<ProcessSale_Section_Item> ProcessSale_Section_Items { get; set; }

    public virtual DbSet<ProcessSale_Section_ItemOption> ProcessSale_Section_ItemOptions { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<System_SLA> System_SLAs { get; set; }

    public virtual DbSet<System_Signature> System_Signatures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<User_Branch> User_Branches { get; set; }

    public virtual DbSet<User_Level> User_Levels { get; set; }

    public virtual DbSet<User_Permission> User_Permissions { get; set; }

    public virtual DbSet<User_Role> User_Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Customer", tb => tb.HasComment("ลูกค้า"));

            entity.Property(e => e.AmphurId)
                .HasComment("อำเภอ")
                .HasColumnType("int(11)");
            entity.Property(e => e.BranchId)
                .HasComment("สาขา")
                .HasColumnType("int(11)");
            entity.Property(e => e.BusinessSize)
                .HasMaxLength(255)
                .HasComment("ขนาดธุรกิจ");
            entity.Property(e => e.BusinessType)
                .HasMaxLength(255)
                .HasComment("ประเภทกิจการ");
            entity.Property(e => e.CompanyEmail)
                .HasMaxLength(255)
                .HasComment("อีเมลบริษัท");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(255)
                .HasComment("ชื่อบริษัท");
            entity.Property(e => e.CompanyTel)
                .HasMaxLength(255)
                .HasComment("โทรศัพท์บริษัท");
            entity.Property(e => e.ContactChannelId)
                .HasComment("ช่องทางการติดต่อ")
                .HasColumnType("int(11)");
            entity.Property(e => e.ContactName)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ติดต่อ");
            entity.Property(e => e.ContactTel)
                .HasMaxLength(255)
                .HasComment("โทรศัพท์");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CreditScore).HasMaxLength(255);
            entity.Property(e => e.DateContact)
                .HasComment("วันที่เข้ามาติดต่อ")
                .HasColumnType("datetime");
            entity.Property(e => e.EmployeeId)
                .HasComment("รหัสพนักงาน")
                .HasColumnType("int(11)");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(255)
                .HasComment("ชื่อพนักงาน");
            entity.Property(e => e.FiscalYear)
                .HasMaxLength(255)
                .HasComment("ปีงบประมาณ");
            entity.Property(e => e.HouseNo)
                .HasMaxLength(255)
                .HasComment("บ้านเลขที่");
            entity.Property(e => e.Inventories)
                .HasMaxLength(255)
                .HasComment("สินค้าคงเหลือ");
            entity.Property(e => e.JuristicPersonRegNumber)
                .HasMaxLength(255)
                .HasComment("เลขทะเบียนนิติบุคคล");
            entity.Property(e => e.MainProduction)
                .HasMaxLength(255)
                .HasComment("ผลผลิตหลัก");
            entity.Property(e => e.NameShareholder)
                .HasMaxLength(255)
                .HasComment("ชื่อผู้ถือหุ้น");
            entity.Property(e => e.Nationality)
                .HasMaxLength(255)
                .HasComment("สัญชาติ");
            entity.Property(e => e.NumberShareholder)
                .HasComment("จำนวนหุ้นที่ถือ")
                .HasColumnType("int(11)");
            entity.Property(e => e.ParentCompanyGroup)
                .HasMaxLength(255)
                .HasComment("กลุ่มบริษัทแม่");
            entity.Property(e => e.Proportion)
                .HasMaxLength(255)
                .HasComment("สัดส่วนการถือหุ้น");
            entity.Property(e => e.ProvinceId)
                .HasComment("จังหวัด")
                .HasColumnType("int(11)");
            entity.Property(e => e.ProvincialOffice)
                .HasMaxLength(255)
                .HasComment("สำนักงานจังหวัด (สนจ.)");
            entity.Property(e => e.RegisteredCapital)
                .HasMaxLength(255)
                .HasComment("ทุนจดทะเบียน");
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
            entity.Property(e => e.TradeAccRecProceedsNet)
                .HasMaxLength(255)
                .HasComment("ลูกหนี้การค้าและตั่วเงินรับ-สุทธิ");
            entity.Property(e => e.TradeAccReceivable)
                .HasMaxLength(255)
                .HasComment("ลูกหนี้การค้า");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.ValueChain)
                .HasMaxLength(255)
                .HasComment("ห่วงโซ่คุณค่า ");
            entity.Property(e => e.VillageNo)
                .HasComment("หมู่ที่")
                .HasColumnType("int(11)");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(255)
                .HasComment("รหัสไปรษณีย์");
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
                .HasMaxLength(255)
                .HasComment("มูลค่าหุ้นทั้งหมด");

            entity.HasOne(d => d.Customer).WithMany(p => p.Customer_Shareholders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_shareholder_ibfk_1");
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

        modelBuilder.Entity<Logging>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("Logging");

            entity.Property(e => e.ClientIp).HasMaxLength(255);
            entity.Property(e => e.ContentType).HasMaxLength(255);
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

        modelBuilder.Entity<Master_Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Branch", tb => tb.HasComment("สาขา"));

            entity.HasIndex(e => e.RegionId, "RegionId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.RegionId).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Region).WithMany(p => p.Master_Branches)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("master_branch_ibfk_1");
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

        modelBuilder.Entity<Master_Division_Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("ฝ่ายกิจการสาขา"));

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasComment("รหัส");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasComment("ชื่อฝ่าย");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Master_Division_Loan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("ฝ่ายธุรกิจสินเชื่อ"));

            entity.HasIndex(e => e.Division_BranchsId, "master_division_loans_ibfk_1");

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasComment("รหัส");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Division_BranchsId).HasComment("สาขาที่ดูแล");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Division_Branchs).WithMany(p => p.Master_Division_Loans)
                .HasForeignKey(d => d.Division_BranchsId)
                .HasConstraintName("master_division_loans_ibfk_1");
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

        modelBuilder.Entity<Master_Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Master_Region", tb => tb.HasComment("ภูมิภาค"));

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
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

        modelBuilder.Entity<ProcessSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale", tb => tb.HasComment("กระบวนการขาย"));

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

        modelBuilder.Entity<ProcessSale_Reply>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale_Reply");

            entity.HasIndex(e => e.ProcessSaleId, "ProcessSaleItemId");

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
        });

        modelBuilder.Entity<ProcessSale_Reply_Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale_Reply_Section");

            entity.HasIndex(e => e.PSaleReplyId, "PSaleReplyId");

            entity.HasIndex(e => e.PSaleSectionId, "PSaleSectionId");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.PSaleReply).WithMany(p => p.ProcessSale_Reply_Sections)
                .HasForeignKey(d => d.PSaleReplyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_reply_section_ibfk_1");

            entity.HasOne(d => d.PSaleSection).WithMany(p => p.ProcessSale_Reply_Sections)
                .HasForeignKey(d => d.PSaleSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_reply_section_ibfk_2");
        });

        modelBuilder.Entity<ProcessSale_Reply_Section_Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale_Reply_Section_Item");

            entity.HasIndex(e => e.PSaleReplySectionId, "ProcessSaleReplyId");

            entity.HasIndex(e => e.PSaleSectionItemId, "processsale_answeritem_ibfk_2");

            entity.Property(e => e.ItemLabel).HasMaxLength(255);
            entity.Property(e => e.ItemType).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.PSaleReplySection).WithMany(p => p.ProcessSale_Reply_Section_Items)
                .HasForeignKey(d => d.PSaleReplySectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_reply_section_item_ibfk_1");

            entity.HasOne(d => d.PSaleSectionItem).WithMany(p => p.ProcessSale_Reply_Section_Items)
                .HasForeignKey(d => d.PSaleSectionItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_reply_section_item_ibfk_2");
        });

        modelBuilder.Entity<ProcessSale_Reply_Section_ItemValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale_Reply_Section_ItemValue");

            entity.HasIndex(e => e.FileId, "FileId");

            entity.HasIndex(e => e.PSaleReplySectionItemId, "PSaleReplySectionItemId");

            entity.HasIndex(e => e.PSaleSectionItemOptionId, "PSaleSectionItemOptionId");

            entity.Property(e => e.FileUrl).HasMaxLength(255);
            entity.Property(e => e.OptionLabel).HasMaxLength(255);
            entity.Property(e => e.ReplyDate).HasColumnType("datetime");
            entity.Property(e => e.ReplyTime).HasColumnType("time");
            entity.Property(e => e.ReplyValue).HasMaxLength(1000);
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.File).WithMany(p => p.ProcessSale_Reply_Section_ItemValues)
                .HasForeignKey(d => d.FileId)
                .HasConstraintName("processsale_reply_section_itemvalue_ibfk_3");

            entity.HasOne(d => d.PSaleReplySectionItem).WithMany(p => p.ProcessSale_Reply_Section_ItemValues)
                .HasForeignKey(d => d.PSaleReplySectionItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_reply_section_itemvalue_ibfk_1");

            entity.HasOne(d => d.PSaleSectionItemOption).WithMany(p => p.ProcessSale_Reply_Section_ItemValues)
                .HasForeignKey(d => d.PSaleSectionItemOptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_reply_section_itemvalue_ibfk_2");
        });

        modelBuilder.Entity<ProcessSale_Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProcessSale_Section");

            entity.HasIndex(e => e.ProcessSaleId, "ProcessSaleId");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.ShowAlways).HasComment("1=แสดงผลตลอด");
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
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
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

            entity.HasIndex(e => e.PSaleSectionItemId, "ProcessSaleItemId");

            entity.Property(e => e.DefaultValue).HasMaxLength(255);
            entity.Property(e => e.OptionLabel).HasMaxLength(255);
            entity.Property(e => e.SequenceNo).HasColumnType("int(11)");
            entity.Property(e => e.ShowSectionId).HasComment("แสดงผลตาม Section");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");

            entity.HasOne(d => d.PSaleSectionItem).WithMany(p => p.ProcessSale_Section_ItemOptions)
                .HasForeignKey(d => d.PSaleSectionItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("processsale_section_itemoption_ibfk_1");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sale");

            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
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

            entity.HasIndex(e => e.LevelId, "LevelId");

            entity.HasIndex(e => e.PositionId, "PositionId");

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EmployeeId).HasMaxLength(10);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.LevelId).HasColumnType("int(11)");
            entity.Property(e => e.LoginFail).HasColumnType("smallint(6)");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PositionId).HasColumnType("int(11)");
            entity.Property(e => e.RoleId).HasColumnType("int(11)");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.TitleName).HasMaxLength(255);
            entity.Property(e => e.UpdateBy).HasColumnType("int(11)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Level).WithMany(p => p.Users)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("user_ibfk_2");

            entity.HasOne(d => d.Position).WithMany(p => p.Users)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("user_ibfk_3");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<User_Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("User_Branch", tb => tb.HasComment("พนักงานในสาขา"));

            entity.HasIndex(e => e.BranchId, "BranchId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)");
            entity.Property(e => e.BranchId).HasColumnType("int(11)");
            entity.Property(e => e.CreateBy).HasColumnType("int(11)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasComment("-1=ลบ  ,0=ไม่ใช้งาน  ,1=ใช้งาน")
                .HasColumnType("smallint(6)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Branch).WithMany(p => p.User_Branches)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_branch_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.User_Branches)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_branch_ibfk_1");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
