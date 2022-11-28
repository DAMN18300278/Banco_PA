using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Banco.Models;

public partial class BancoContext : DbContext
{
    public BancoContext()
    {
    }

    public BancoContext(DbContextOptions<BancoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cuentum> Cuenta { get; set; }

    public virtual DbSet<DatosPrestamo> DatosPrestamos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Gerente> Gerentes { get; set; }

    public virtual DbSet<Historial> Historials { get; set; }

    public virtual DbSet<Rifa> Rifas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=banco; Integrated Security=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cuentum>(entity =>
        {
            entity.HasKey(e => e.NumCuenta).HasName("PK__cuenta__1B645EA9A21D1F99");

            entity.ToTable("cuenta");

            entity.Property(e => e.NumCuenta)
                .ValueGeneratedOnAdd()
                .HasColumnName("Num_Cuenta");
            entity.Property(e => e.Saldo).HasDefaultValueSql("((10000))");
            entity.Property(e => e.Usuario)
                .HasMaxLength(19)
                .IsUnicode(false);

            entity.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.Usuario)
                .HasConstraintName("cuentaUsuario_usuarioCURP");
        });

        modelBuilder.Entity<DatosPrestamo>(entity =>
        {
            entity.HasKey(e => e.NumFolio).HasName("PK__datos_pr__40975068A8646531");

            entity.ToTable("datos_prestamos");

            entity.Property(e => e.NumFolio).HasColumnName("Num_Folio");
            entity.Property(e => e.FechaAprobacion)
                .HasColumnType("date")
                .HasColumnName("Fecha_Aprobacion");
            entity.Property(e => e.FechaLimite)
                .HasColumnType("date")
                .HasColumnName("Fecha_Limite");
            entity.Property(e => e.FechaLiquidacion)
                .HasColumnType("date")
                .HasColumnName("Fecha_Liquidacion");
            entity.Property(e => e.FechaSolicitud)
                .HasColumnType("date")
                .HasColumnName("Fecha_Solicitud");
            entity.Property(e => e.NumHistorial).HasColumnName("Num_Historial");

            entity.HasOne(d => d.NumHistorialNavigation).WithMany(p => p.DatosPrestamos)
                .HasForeignKey(d => d.NumHistorial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("datosFolio_historialFolio");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Nomina).HasName("PK__empleado__765BE2D801DA2A42");

            entity.ToTable("empleado");

            entity.Property(e => e.Nomina).ValueGeneratedNever();
            entity.Property(e => e.Curp)
                .HasMaxLength(19)
                .IsUnicode(false)
                .HasColumnName("CURP");

            entity.HasOne(d => d.CurpNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.Curp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("empleadoCURP_usuarioCURP");
        });

        modelBuilder.Entity<Gerente>(entity =>
        {
            entity.HasKey(e => e.NumNom).HasName("PK__gerentes__E478FBEFC512E8F7");

            entity.ToTable("gerentes");

            entity.Property(e => e.NumNom).HasColumnName("Num_Nom");
            entity.Property(e => e.DiasVacaciones).HasColumnName("Dias_Vacaciones");

            entity.HasOne(d => d.NominaNavigation).WithMany(p => p.Gerentes)
                .HasForeignKey(d => d.Nomina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("gerentesNomina_empleadoNomina");
        });

        modelBuilder.Entity<Historial>(entity =>
        {
            entity.HasKey(e => e.NumHistorial).HasName("PK__historia__9ECF02967BBF8967");

            entity.ToTable("historial");

            entity.Property(e => e.NumHistorial)
                .ValueGeneratedNever()
                .HasColumnName("Num_Historial");
            entity.Property(e => e.NominaEncargado).HasColumnName("Nomina_Encargado");
            entity.Property(e => e.NumCuenta).HasColumnName("Num_Cuenta");
            entity.Property(e => e.PagoPendientes).HasColumnName("Pago_Pendientes");
            entity.Property(e => e.PagoRealizados).HasColumnName("Pago_Realizados");

            entity.HasOne(d => d.NominaEncargadoNavigation).WithMany(p => p.Historials)
                .HasForeignKey(d => d.NominaEncargado)
                .HasConstraintName("historialAprobado_empleadoNomina");

            entity.HasOne(d => d.NumCuentaNavigation).WithMany(p => p.Historials)
                .HasForeignKey(d => d.NumCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("historialSolicitado_cuentaNoCuenta");
        });

        modelBuilder.Entity<Rifa>(entity =>
        {
            entity.HasKey(e => e.NumBoleto).HasName("PK__rifa__D3378588CDA48775");

            entity.ToTable("rifa");

            entity.Property(e => e.NumBoleto).HasColumnName("Num_Boleto");
            entity.Property(e => e.FechaBoleto)
                .HasColumnType("date")
                .HasColumnName("Fecha_Boleto");

            entity.HasOne(d => d.CuentaNavigation).WithMany(p => p.Rifas)
                .HasForeignKey(d => d.Cuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rifaCuenta_cuentaNoCuenta");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Curp).HasName("PK__usuario__F46C4CBE8D2D94B3");

            entity.ToTable("usuario");

            entity.Property(e => e.Curp)
                .HasMaxLength(19)
                .IsUnicode(false)
                .HasColumnName("CURP");
            entity.Property(e => e.ApellidoM)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Apellido_M");
            entity.Property(e => e.ApellidoP)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Apellido_P");
            entity.Property(e => e.Cumpleaños).HasColumnType("date");
            entity.Property(e => e.NombreS)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Nombre(s)");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Contraseña");
            entity.Property(e => e.Autorizada)
                .IsUnicode(false)
                .HasColumnName("Autorizada")
                .HasDefaultValueSql("((0))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
