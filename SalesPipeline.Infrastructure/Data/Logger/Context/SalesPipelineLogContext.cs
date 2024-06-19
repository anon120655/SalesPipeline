using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SalesPipeline.Infrastructure.Data.Logger.Entity;

namespace SalesPipeline.Infrastructure.Data.Logger.Context;

public partial class SalesPipelineLogContext : DbContext
{
    public SalesPipelineLogContext(DbContextOptions<SalesPipelineLogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Logging> Loggings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
