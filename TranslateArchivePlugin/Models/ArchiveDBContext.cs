using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TranslateArchivePlugin.Models.DB;

namespace TranslateArchivePlugin.Models
{
    public class ArchiveDBContext : DbContext
    {
        public ArchiveDBContext(DbContextOptions<ArchiveDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbFileLibrary> TbFileLibrary { get; set; }
        public virtual DbSet<TbTranslateArchive> TbTranslateArchive { get; set; }
        public virtual DbSet<TbTranslateHistory> TbTranslateHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbFileLibrary>(entity =>
            {
                entity.ToTable("tbfilelibrary");

                entity.HasKey(e => e.FileId);

                entity.Property(e => e.FileId)
                    .HasColumnName("FileId")
                    .HasColumnType("int(11)")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FilePath)
                    .HasColumnName("FilePath")
                    .HasColumnType("varchar(2000)")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<TbTranslateArchive>(entity =>
            {
                entity.ToTable("tbtranslatearchive");

                entity.HasKey(e => e.TId);

                entity.HasIndex(e => e.FileId)
                    .HasName("IDX_FILEID");

                entity.Property(e => e.TId)
                    .HasColumnName("TId")
                    .HasColumnType("int(11)")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FileId)
                    .HasColumnName("FileId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Trunk)
                    .HasColumnName("Trunk")
                    .HasColumnType("varchar(64)");

                entity.Property(e => e.LogId)
                    .HasColumnName("LogId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<TbTranslateHistory>(entity =>
            {
                entity.ToTable("tbtranslatehistory");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.TId)
                    .HasName("IDX_TID");

                entity.Property(e => e.Id)
                    .HasColumnName("Id")
                    .HasColumnType("int(11)")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Type)
                    .HasColumnName("Type")
                    .HasColumnType("int(11)");
                
                entity.Property(e => e.Time)
                   .HasColumnName("Time")
                   .HasColumnType("DATETIME")
                   .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.User)
                    .HasColumnName("User")
                    .HasColumnType("varchar(64)")
                    .HasDefaultValueSql("''");
            });
        }
    }
}
