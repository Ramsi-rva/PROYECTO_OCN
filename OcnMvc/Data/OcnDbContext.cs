using Microsoft.EntityFrameworkCore;
using OcnMvc.Models;

namespace OcnMvc.Data;

// Contexto de Entity Framework: conecta las clases con las tablas del esquema ocn
public class OcnDbContext : DbContext
{
    public OcnDbContext(DbContextOptions<OcnDbContext> options) : base(options) { }

    public DbSet<DivisionTerritorial> DivisionTerritorial => Set<DivisionTerritorial>();
    public DbSet<CatTipoZona> CatTipoZona => Set<CatTipoZona>();
    public DbSet<Zona> Zona => Set<Zona>();
    public DbSet<CategoriaProblematica> CategoriaProblematica => Set<CategoriaProblematica>();
    public DbSet<Problematica> Problematica => Set<Problematica>();
    public DbSet<RegistroOCN> RegistroOCN => Set<RegistroOCN>();

    // Define el nombre de cada tabla, sus columnas, llaves y relaciones
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DivisionTerritorial>(e =>
        {
            e.ToTable("divisionterritorial", "ocn");
            e.HasKey(x => x.IdDivision);
            e.Property(x => x.IdDivision).HasColumnName("iddivision").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(60).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<CatTipoZona>(e =>
        {
            e.ToTable("cattipozona", "ocn");
            e.HasKey(x => x.IdTipoZona);
            e.Property(x => x.IdTipoZona).HasColumnName("idtipozona").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Zona>(e =>
        {
            e.ToTable("zona", "ocn");
            e.HasKey(x => x.IdZona);
            e.Property(x => x.IdZona).HasColumnName("idzona").ValueGeneratedOnAdd();
            e.Property(x => x.CodigoZona).HasColumnName("codigozona").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.IdDivision).HasColumnName("iddivision");
            e.Property(x => x.IdTipoZona).HasColumnName("idtipozona");
            e.Property(x => x.DescripcionGeneral).HasColumnName("descripciongeneral").HasMaxLength(500).IsUnicode(false);
            e.Property(x => x.ReferenciaGeograficaAprox).HasColumnName("referenciageograficaaprox").HasMaxLength(200).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
            e.HasOne(x => x.Division).WithMany().HasForeignKey(x => x.IdDivision).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.TipoZona).WithMany().HasForeignKey(x => x.IdTipoZona).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CategoriaProblematica>(e =>
        {
            e.ToTable("categoriaproblematica", "ocn");
            e.HasKey(x => x.IdCategoria);
            e.Property(x => x.IdCategoria).HasColumnName("idcategoria").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Problematica>(e =>
        {
            e.ToTable("problematica", "ocn");
            e.HasKey(x => x.IdProblematica);
            e.Property(x => x.IdProblematica).HasColumnName("idproblematica").ValueGeneratedOnAdd();
            e.Property(x => x.IdCategoria).HasColumnName("idcategoria");
            e.Property(x => x.IdProblematicaPadre).HasColumnName("idproblematicapadre");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200).IsUnicode(false);
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(500).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
            e.HasOne(x => x.Categoria).WithMany().HasForeignKey(x => x.IdCategoria).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.ProblematicaPadre).WithMany().HasForeignKey(x => x.IdProblematicaPadre).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RegistroOCN>(e =>
        {
            e.ToTable("registroocn", "ocn");
            e.HasKey(x => x.IdRegistroOCN);
            e.Property(x => x.IdRegistroOCN).HasColumnName("idregistroocn").ValueGeneratedOnAdd();
            e.Property(x => x.IdZona).HasColumnName("idzona");
            e.Property(x => x.IdProblematica).HasColumnName("idproblematica");
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(500).IsUnicode(false);
            e.Property(x => x.FechaRegistro).HasColumnName("fecharegistro").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
            e.Property(x => x.Activo).HasColumnName("activo");
            e.HasOne(x => x.Zona).WithMany().HasForeignKey(x => x.IdZona).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Problematica).WithMany().HasForeignKey(x => x.IdProblematica).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
