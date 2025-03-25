using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace RepMed.Data;

public partial class RepMedContext : DbContext
{
    public RepMedContext()
    {
    }

    public RepMedContext(DbContextOptions<RepMedContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Coderequest> Coderequests { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Person> Persons { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolepermission> Rolepermissions { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usercontact> Usercontacts { get; set; }

    public virtual DbSet<Userrolepermission> Userrolepermissions { get; set; }

    public virtual DbSet<Usertokenlog> Usertokenlogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=repmeddb;user=root;password=Hitesh", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("city");

            entity.HasIndex(e => e.StateId, "StateID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.StateId).HasColumnName("StateID");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("city_ibfk_1");
        });

        modelBuilder.Entity<Coderequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("coderequest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SecurityCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Userid).HasMaxLength(450);
            entity.Property(e => e.ValidTo).HasColumnType("timestamp(3)");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryCode).HasMaxLength(10);
            entity.Property(e => e.CountryCodeTwo).HasMaxLength(5);
            entity.Property(e => e.Currency).HasMaxLength(20);
            entity.Property(e => e.CurrencySymbol).HasMaxLength(10);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.TimeZone)
                .HasMaxLength(255)
                .HasDefaultValueSql("''");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("persons");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BloodGroup).HasMaxLength(10);
            entity.Property(e => e.CreatedOn).HasColumnType("timestamp(3)");
            entity.Property(e => e.Dob)
                .HasColumnType("timestamp(3)")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(500);
            entity.Property(e => e.FirstName).HasMaxLength(500);
            entity.Property(e => e.LastName).HasMaxLength(500);
            entity.Property(e => e.Mobile).HasMaxLength(100);
            entity.Property(e => e.MotherName).HasMaxLength(500);
            entity.Property(e => e.Picture).HasMaxLength(500);
            entity.Property(e => e.Qualifications).HasMaxLength(500);
            entity.Property(e => e.Signatures).HasMaxLength(500);
            entity.Property(e => e.State).HasMaxLength(500);
            entity.Property(e => e.UpdatedOn).HasColumnType("timestamp(3)");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
        });

        modelBuilder.Entity<Rolepermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rolepermissions");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Action).HasMaxLength(500);
            entity.Property(e => e.Controller).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(1500);
            entity.Property(e => e.IsApis)
                .HasDefaultValueSql("'0'")
                .HasColumnName("IsAPIs");
            entity.Property(e => e.Parma).HasMaxLength(500);
            entity.Property(e => e.Permission).HasMaxLength(500);
            entity.Property(e => e.Route).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(500);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("state");

            entity.HasIndex(e => e.CountryId, "CountryID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Name).HasMaxLength(500);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("state_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.PersonId, "PersonId");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ein)
                .HasMaxLength(25)
                .HasColumnName("EIN");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.LastLoginDate).HasColumnType("timestamp(3)");
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasColumnType("text");
            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasColumnType("text");
            entity.Property(e => e.Ssn)
                .HasMaxLength(25)
                .HasColumnName("SSN");

            entity.HasOne(d => d.Person).WithMany(p => p.Users)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_ibfk_1");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Userrole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("userroles_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("userroles_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("userroles");
                        j.HasIndex(new[] { "RoleId" }, "RoleID");
                        j.IndexerProperty<Guid>("UserId").HasColumnName("UserID");
                        j.IndexerProperty<int>("RoleId").HasColumnName("RoleID");
                    });
        });

        modelBuilder.Entity<Usercontact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usercontacts");

            entity.HasIndex(e => e.PersonId, "PersonID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Address1).HasMaxLength(500);
            entity.Property(e => e.Fax).HasMaxLength(50);
            entity.Property(e => e.LandMark).HasMaxLength(200);
            entity.Property(e => e.Latitude).HasMaxLength(50);
            entity.Property(e => e.Longitude).HasMaxLength(50);
            entity.Property(e => e.MobileNo).HasMaxLength(50);
            entity.Property(e => e.MobileNo2).HasMaxLength(50);
            entity.Property(e => e.PersonId).HasColumnName("PersonID");

            entity.HasOne(d => d.Person).WithMany(p => p.Usercontacts)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("usercontacts_ibfk_1");
        });

        modelBuilder.Entity<Userrolepermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("userrolepermission");

            entity.HasIndex(e => e.PermissionId, "PermissionId");

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Permission).WithMany(p => p.Userrolepermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("userrolepermission_ibfk_1");

            entity.HasOne(d => d.Role).WithMany(p => p.Userrolepermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("userrolepermission_ibfk_2");
        });

        modelBuilder.Entity<Usertokenlog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usertokenlog");

            entity.HasIndex(e => e.UserId, "UserID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedDate).HasColumnType("timestamp(3)");
            entity.Property(e => e.Token)
                .IsRequired()
                .HasColumnType("text");
            entity.Property(e => e.TokenValidTill).HasColumnType("timestamp(3)");
            entity.Property(e => e.UpdatedDate).HasColumnType("timestamp(3)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Usertokenlogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("usertokenlog_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
