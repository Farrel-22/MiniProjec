using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MiniProject.DAL.ScaffoldContainer;

public partial class Latihan1Context : DbContext
{
    public Latihan1Context()
    {
    }

    public Latihan1Context(DbContextOptions<Latihan1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<MKecamatan> MKecamatans { get; set; }

    public virtual DbSet<MKelurahan> MKelurahans { get; set; }

    public virtual DbSet<MKodepos> MKodepos { get; set; }

    public virtual DbSet<MKota> MKota { get; set; }

    public virtual DbSet<TrBudget> TrBudgets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost; Database=Latihan1; User ID=postgres; Password=Lazzuard;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MKecamatan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("m_kecamatan_pkey");

            entity.ToTable("m_kecamatan");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Guid).HasColumnName("guid");
            entity.Property(e => e.IdKota).HasColumnName("id_kota");
            entity.Property(e => e.Nama)
                .HasMaxLength(150)
                .HasColumnName("nama");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(d => d.IdKotaNavigation).WithMany(p => p.MKecamatans)
                .HasForeignKey(d => d.IdKota)
                .HasConstraintName("fk_kecamatan_kota");
        });

        modelBuilder.Entity<MKelurahan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("m_kelurahan_pkey");

            entity.ToTable("m_kelurahan");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Guid).HasColumnName("guid");
            entity.Property(e => e.IdKecamatan).HasColumnName("id_kecamatan");
            entity.Property(e => e.Nama)
                .HasMaxLength(150)
                .HasColumnName("nama");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(d => d.IdKecamatanNavigation).WithMany(p => p.MKelurahans)
                .HasForeignKey(d => d.IdKecamatan)
                .HasConstraintName("fk_kelurahan_kecamatan");
        });

        modelBuilder.Entity<MKodepos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("m_kodepos_pkey");

            entity.ToTable("m_kodepos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Guid).HasColumnName("guid");
            entity.Property(e => e.IdKelurahan).HasColumnName("id_kelurahan");
            entity.Property(e => e.Kodepos)
                .HasMaxLength(10)
                .HasColumnName("kodepos");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(d => d.IdKelurahanNavigation).WithMany(p => p.MKodepos)
                .HasForeignKey(d => d.IdKelurahan)
                .HasConstraintName("fk_kodepos_kelurahan");
        });

        modelBuilder.Entity<MKota>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("m_kota_pkey");

            entity.ToTable("m_kota");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Guid).HasColumnName("guid");
            entity.Property(e => e.Nama)
                .HasMaxLength(150)
                .HasColumnName("nama");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
        }); 

        modelBuilder.Entity<TrBudget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("t_bugdet_pkey");

            entity.ToTable("tr_budget");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('t_bugdet_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Addition)
                .HasPrecision(15, 2)
                .HasDefaultValue(0m)
                .HasColumnName("addition");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.Deduction)
                .HasPrecision(15, 2)
                .HasDefaultValue(0m)
                .HasColumnName("deduction");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Guid).HasColumnName("guid");
            entity.Property(e => e.IdKodepos).HasColumnName("id_kodepos");
            entity.Property(e => e.JudulTransaksi)
                .HasMaxLength(100)
                .HasColumnName("judul_transaksi");
            entity.Property(e => e.Nominal)
                .HasPrecision(15, 2)
                .HasDefaultValue(0m)
                .HasColumnName("nominal");
            entity.Property(e => e.OpenBalance)
                .HasPrecision(15, 2)
                .HasDefaultValue(0m)
                .HasColumnName("open_balance");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100)
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(d => d.IdKodeposNavigation).WithMany(p => p.TrBudgets)
                .HasForeignKey(d => d.IdKodepos)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_budget_kodepos");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
