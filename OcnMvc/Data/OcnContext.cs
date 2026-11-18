using Microsoft.EntityFrameworkCore;
using OcnMvc.Models;

namespace OcnMvc.Data;

// Contexto de Entity Framework: conecta las clases con las tablas del esquema ocn
public class OcnContext : DbContext
{
    public OcnContext(DbContextOptions<OcnContext> options) : base(options) { }

    public DbSet<Rol> Rol => Set<Rol>();
    public DbSet<Usuario> Usuario => Set<Usuario>();
    public DbSet<UsuarioRol> UsuarioRol => Set<UsuarioRol>();
    public DbSet<DivisionTerritorial> DivisionTerritorial => Set<DivisionTerritorial>();
    public DbSet<CatTipoZona> CatTipoZona => Set<CatTipoZona>();
    public DbSet<Zona> Zona => Set<Zona>();
    public DbSet<Periodo> Periodo => Set<Periodo>();
    public DbSet<CatMetodo> CatMetodo => Set<CatMetodo>();
    public DbSet<Instrumento> Instrumento => Set<Instrumento>();
    public DbSet<Levantamiento> Levantamiento => Set<Levantamiento>();
    public DbSet<LevantamientoInstrumento> LevantamientoInstrumento => Set<LevantamientoInstrumento>();
    public DbSet<LevantamientoResponsable> LevantamientoResponsable => Set<LevantamientoResponsable>();
    public DbSet<CatTipoFuente> CatTipoFuente => Set<CatTipoFuente>();
    public DbSet<CatTipoOrganizacion> CatTipoOrganizacion => Set<CatTipoOrganizacion>();
    public DbSet<Organizacion> Organizacion => Set<Organizacion>();
    public DbSet<Fuente> Fuente => Set<Fuente>();
    public DbSet<LevantamientoFuente> LevantamientoFuente => Set<LevantamientoFuente>();
    public DbSet<Observacion> Observacion => Set<Observacion>();
    public DbSet<ObservacionFuente> ObservacionFuente => Set<ObservacionFuente>();
    public DbSet<CatGrupoPoblacional> CatGrupoPoblacional => Set<CatGrupoPoblacional>();
    public DbSet<ObservacionGrupoPoblacional> ObservacionGrupoPoblacional => Set<ObservacionGrupoPoblacional>();
    public DbSet<CategoriaProblematica> CategoriaProblematica => Set<CategoriaProblematica>();
    public DbSet<Problematica> Problematica => Set<Problematica>();
    public DbSet<CatMagnitud> CatMagnitud => Set<CatMagnitud>();
    public DbSet<CatGravedad> CatGravedad => Set<CatGravedad>();
    public DbSet<CatFrecuencia> CatFrecuencia => Set<CatFrecuencia>();
    public DbSet<CatTendencia> CatTendencia => Set<CatTendencia>();
    public DbSet<ObservacionProblematica> ObservacionProblematica => Set<ObservacionProblematica>();
    public DbSet<TendenciaProblematica> TendenciaProblematica => Set<TendenciaProblematica>();
    public DbSet<Necesidad> Necesidad => Set<Necesidad>();
    public DbSet<ProblematicaNecesidad> ProblematicaNecesidad => Set<ProblematicaNecesidad>();
    public DbSet<CatFactorRiesgo> CatFactorRiesgo => Set<CatFactorRiesgo>();
    public DbSet<ProblematicaFactorRiesgo> ProblematicaFactorRiesgo => Set<ProblematicaFactorRiesgo>();
    public DbSet<CatFactorProtector> CatFactorProtector => Set<CatFactorProtector>();
    public DbSet<ProblematicaFactorProtector> ProblematicaFactorProtector => Set<ProblematicaFactorProtector>();
    public DbSet<CatTipoRecurso> CatTipoRecurso => Set<CatTipoRecurso>();
    public DbSet<CatEstadoRecurso> CatEstadoRecurso => Set<CatEstadoRecurso>();
    public DbSet<CatCostoRecurso> CatCostoRecurso => Set<CatCostoRecurso>();
    public DbSet<Recurso> Recurso => Set<Recurso>();
    public DbSet<FactorProtectorRecurso> FactorProtectorRecurso => Set<FactorProtectorRecurso>();
    public DbSet<CatServicio> CatServicio => Set<CatServicio>();
    public DbSet<RecursoServicio> RecursoServicio => Set<RecursoServicio>();
    public DbSet<ServicioNecesidad> ServicioNecesidad => Set<ServicioNecesidad>();
    public DbSet<CatTipoCobertura> CatTipoCobertura => Set<CatTipoCobertura>();
    public DbSet<RecursoZonaCobertura> RecursoZonaCobertura => Set<RecursoZonaCobertura>();
    public DbSet<CatBarrera> CatBarrera => Set<CatBarrera>();
    public DbSet<RecursoZonaBarrera> RecursoZonaBarrera => Set<RecursoZonaBarrera>();
    public DbSet<RecursoZonaAccesibilidad> RecursoZonaAccesibilidad => Set<RecursoZonaAccesibilidad>();
    public DbSet<CatTipoEvidencia> CatTipoEvidencia => Set<CatTipoEvidencia>();
    public DbSet<Evidencia> Evidencia => Set<Evidencia>();
    public DbSet<CatTipoIntervencion> CatTipoIntervencion => Set<CatTipoIntervencion>();
    public DbSet<NecesidadTipoIntervencion> NecesidadTipoIntervencion => Set<NecesidadTipoIntervencion>();
    public DbSet<TipoIntervencionServicio> TipoIntervencionServicio => Set<TipoIntervencionServicio>();
    public DbSet<ModeloPriorizacion> ModeloPriorizacion => Set<ModeloPriorizacion>();
    public DbSet<ModeloPonderacion> ModeloPonderacion => Set<ModeloPonderacion>();
    public DbSet<PrioridadCalculada> PrioridadCalculada => Set<PrioridadCalculada>();
    public DbSet<BrechaAtencion> BrechaAtencion => Set<BrechaAtencion>();
    public DbSet<Auditoria> Auditoria => Set<Auditoria>();

    // Define el nombre de cada tabla, sus columnas, llaves y relaciones
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>(e =>
        {
            e.ToTable("rol", "ocn");
            e.HasKey(x => x.IdRol);
            e.Property(x => x.IdRol).HasColumnName("idrol").ValueGeneratedOnAdd();
            e.Property(x => x.NombreRol).HasColumnName("nombrerol").HasMaxLength(50).IsUnicode(false);
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(200).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("usuario", "ocn");
            e.HasKey(x => x.IdUsuario);
            e.Property(x => x.IdUsuario).HasColumnName("idusuario").ValueGeneratedOnAdd();
            e.Property(x => x.NombreUsuario).HasColumnName("nombreusuario").HasMaxLength(50).IsUnicode(false);
            e.Property(x => x.NombreCompleto).HasColumnName("nombrecompleto").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.CorreoInstitucional).HasColumnName("correoinstitucional").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
            e.Property(x => x.FechaAlta).HasColumnName("fechaalta").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
        });

        modelBuilder.Entity<UsuarioRol>(e =>
        {
            e.ToTable("usuariorol", "ocn");
            e.HasKey(x => new { x.IdUsuario, x.IdRol });
            e.Property(x => x.IdUsuario).HasColumnName("idusuario").ValueGeneratedNever();
            e.Property(x => x.IdRol).HasColumnName("idrol").ValueGeneratedNever();
            e.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Rol).WithMany().HasForeignKey(x => x.IdRol).OnDelete(DeleteBehavior.Restrict);
        });

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

        modelBuilder.Entity<Periodo>(e =>
        {
            e.ToTable("periodo", "ocn");
            e.HasKey(x => x.IdPeriodo);
            e.Property(x => x.IdPeriodo).HasColumnName("idperiodo").ValueGeneratedOnAdd();
            e.Property(x => x.CodigoPeriodo).HasColumnName("codigoperiodo").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(false);
            e.Property(x => x.FechaInicio).HasColumnName("fechainicio").HasColumnType("date");
            e.Property(x => x.FechaFin).HasColumnName("fechafin").HasColumnType("date");
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<CatMetodo>(e =>
        {
            e.ToTable("catmetodo", "ocn");
            e.HasKey(x => x.IdMetodo);
            e.Property(x => x.IdMetodo).HasColumnName("idmetodo").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(60).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Instrumento>(e =>
        {
            e.ToTable("instrumento", "ocn");
            e.HasKey(x => x.IdInstrumento);
            e.Property(x => x.IdInstrumento).HasColumnName("idinstrumento").ValueGeneratedOnAdd();
            e.Property(x => x.NombreFamilia).HasColumnName("nombrefamilia").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.Version).HasColumnName("version").HasMaxLength(20).IsUnicode(false);
            e.Property(x => x.IdMetodo).HasColumnName("idmetodo");
            e.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.IdInstrumentoAnterior).HasColumnName("idinstrumentoanterior");
            e.Property(x => x.FechaVersion).HasColumnName("fechaversion").HasColumnType("date");
            e.Property(x => x.Activo).HasColumnName("activo");
            e.HasOne(x => x.Metodo).WithMany().HasForeignKey(x => x.IdMetodo).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.InstrumentoAnterior).WithMany().HasForeignKey(x => x.IdInstrumentoAnterior).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Levantamiento>(e =>
        {
            e.ToTable("levantamiento", "ocn");
            e.HasKey(x => x.IdLevantamiento);
            e.Property(x => x.IdLevantamiento).HasColumnName("idlevantamiento").ValueGeneratedOnAdd();
            e.Property(x => x.CodigoLevantamiento).HasColumnName("codigolevantamiento").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.IdZona).HasColumnName("idzona");
            e.Property(x => x.IdPeriodo).HasColumnName("idperiodo");
            e.Property(x => x.IdMetodo).HasColumnName("idmetodo");
            e.Property(x => x.FechaLevantamiento).HasColumnName("fechalevantamiento").HasColumnType("date");
            e.Property(x => x.NumeroParticipantesAprox).HasColumnName("numeroparticipantesaprox");
            e.Property(x => x.ObservacionesMetodologicas).HasColumnName("observacionesmetodologicas").HasMaxLength(1000).IsUnicode(false);
            e.Property(x => x.FechaCaptura).HasColumnName("fechacaptura").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
            e.HasOne(x => x.Zona).WithMany().HasForeignKey(x => x.IdZona).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Periodo).WithMany().HasForeignKey(x => x.IdPeriodo).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Metodo).WithMany().HasForeignKey(x => x.IdMetodo).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LevantamientoInstrumento>(e =>
        {
            e.ToTable("levantamientoinstrumento", "ocn");
            e.HasKey(x => new { x.IdLevantamiento, x.IdInstrumento });
            e.Property(x => x.IdLevantamiento).HasColumnName("idlevantamiento").ValueGeneratedNever();
            e.Property(x => x.IdInstrumento).HasColumnName("idinstrumento").ValueGeneratedNever();
            e.HasOne(x => x.Levantamiento).WithMany().HasForeignKey(x => x.IdLevantamiento).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Instrumento).WithMany().HasForeignKey(x => x.IdInstrumento).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LevantamientoResponsable>(e =>
        {
            e.ToTable("levantamientoresponsable", "ocn");
            e.HasKey(x => new { x.IdLevantamiento, x.IdUsuario });
            e.Property(x => x.IdLevantamiento).HasColumnName("idlevantamiento").ValueGeneratedNever();
            e.Property(x => x.IdUsuario).HasColumnName("idusuario").ValueGeneratedNever();
            e.Property(x => x.RolEnLevantamiento).HasColumnName("rolenlevantamiento").HasMaxLength(50).IsUnicode(false);
            e.HasOne(x => x.Levantamiento).WithMany().HasForeignKey(x => x.IdLevantamiento).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatTipoFuente>(e =>
        {
            e.ToTable("cattipofuente", "ocn");
            e.HasKey(x => x.IdTipoFuente);
            e.Property(x => x.IdTipoFuente).HasColumnName("idtipofuente").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(60).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<CatTipoOrganizacion>(e =>
        {
            e.ToTable("cattipoorganizacion", "ocn");
            e.HasKey(x => x.IdTipoOrganizacion);
            e.Property(x => x.IdTipoOrganizacion).HasColumnName("idtipoorganizacion").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(60).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Organizacion>(e =>
        {
            e.ToTable("organizacion", "ocn");
            e.HasKey(x => x.IdOrganizacion);
            e.Property(x => x.IdOrganizacion).HasColumnName("idorganizacion").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200).IsUnicode(false);
            e.Property(x => x.IdTipoOrganizacion).HasColumnName("idtipoorganizacion");
            e.Property(x => x.Activo).HasColumnName("activo");
            e.HasOne(x => x.TipoOrganizacion).WithMany().HasForeignKey(x => x.IdTipoOrganizacion).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Fuente>(e =>
        {
            e.ToTable("fuente", "ocn");
            e.HasKey(x => x.IdFuente);
            e.Property(x => x.IdFuente).HasColumnName("idfuente").ValueGeneratedOnAdd();
            e.Property(x => x.IdTipoFuente).HasColumnName("idtipofuente");
            e.Property(x => x.IdOrganizacion).HasColumnName("idorganizacion");
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(300).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
            e.HasOne(x => x.TipoFuente).WithMany().HasForeignKey(x => x.IdTipoFuente).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Organizacion).WithMany().HasForeignKey(x => x.IdOrganizacion).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LevantamientoFuente>(e =>
        {
            e.ToTable("levantamientofuente", "ocn");
            e.HasKey(x => new { x.IdLevantamiento, x.IdFuente });
            e.Property(x => x.IdLevantamiento).HasColumnName("idlevantamiento").ValueGeneratedNever();
            e.Property(x => x.IdFuente).HasColumnName("idfuente").ValueGeneratedNever();
            e.HasOne(x => x.Levantamiento).WithMany().HasForeignKey(x => x.IdLevantamiento).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Fuente).WithMany().HasForeignKey(x => x.IdFuente).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Observacion>(e =>
        {
            e.ToTable("observacion", "ocn");
            e.HasKey(x => x.IdObservacion);
            e.Property(x => x.IdObservacion).HasColumnName("idobservacion").ValueGeneratedOnAdd();
            e.Property(x => x.IdLevantamiento).HasColumnName("idlevantamiento");
            e.Property(x => x.NotaContexto).HasColumnName("notacontexto").HasMaxLength(500).IsUnicode(false);
            e.Property(x => x.FechaRegistro).HasColumnName("fecharegistro").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
            e.HasOne(x => x.Levantamiento).WithMany().HasForeignKey(x => x.IdLevantamiento).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ObservacionFuente>(e =>
        {
            e.ToTable("observacionfuente", "ocn");
            e.HasKey(x => new { x.IdObservacion, x.IdFuente });
            e.Property(x => x.IdObservacion).HasColumnName("idobservacion").ValueGeneratedNever();
            e.Property(x => x.IdFuente).HasColumnName("idfuente").ValueGeneratedNever();
            e.HasOne(x => x.Observacion).WithMany().HasForeignKey(x => x.IdObservacion).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Fuente).WithMany().HasForeignKey(x => x.IdFuente).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatGrupoPoblacional>(e =>
        {
            e.ToTable("catgrupopoblacional", "ocn");
            e.HasKey(x => x.IdGrupoPoblacional);
            e.Property(x => x.IdGrupoPoblacional).HasColumnName("idgrupopoblacional").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(false);
            e.Property(x => x.RangoEdad).HasColumnName("rangoedad").HasMaxLength(20).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<ObservacionGrupoPoblacional>(e =>
        {
            e.ToTable("observaciongrupopoblacional", "ocn");
            e.HasKey(x => new { x.IdObservacion, x.IdGrupoPoblacional });
            e.Property(x => x.IdObservacion).HasColumnName("idobservacion").ValueGeneratedNever();
            e.Property(x => x.IdGrupoPoblacional).HasColumnName("idgrupopoblacional").ValueGeneratedNever();
            e.HasOne(x => x.Observacion).WithMany().HasForeignKey(x => x.IdObservacion).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.GrupoPoblacional).WithMany().HasForeignKey(x => x.IdGrupoPoblacional).OnDelete(DeleteBehavior.Restrict);
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

        modelBuilder.Entity<CatMagnitud>(e =>
        {
            e.ToTable("catmagnitud", "ocn");
            e.HasKey(x => x.IdMagnitud);
            e.Property(x => x.IdMagnitud).HasColumnName("idmagnitud").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(40).IsUnicode(false);
            e.Property(x => x.Orden).HasColumnName("orden");
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<CatGravedad>(e =>
        {
            e.ToTable("catgravedad", "ocn");
            e.HasKey(x => x.IdGravedad);
            e.Property(x => x.IdGravedad).HasColumnName("idgravedad").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(40).IsUnicode(false);
            e.Property(x => x.Orden).HasColumnName("orden");
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<CatFrecuencia>(e =>
        {
            e.ToTable("catfrecuencia", "ocn");
            e.HasKey(x => x.IdFrecuencia);
            e.Property(x => x.IdFrecuencia).HasColumnName("idfrecuencia").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(40).IsUnicode(false);
            e.Property(x => x.Orden).HasColumnName("orden");
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<CatTendencia>(e =>
        {
            e.ToTable("cattendencia", "ocn");
            e.HasKey(x => x.IdTendencia);
            e.Property(x => x.IdTendencia).HasColumnName("idtendencia").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(40).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<ObservacionProblematica>(e =>
        {
            e.ToTable("observacionproblematica", "ocn");
            e.HasKey(x => x.IdObservacionProblematica);
            e.Property(x => x.IdObservacionProblematica).HasColumnName("idobservacionproblematica").ValueGeneratedOnAdd();
            e.Property(x => x.IdObservacion).HasColumnName("idobservacion");
            e.Property(x => x.IdProblematica).HasColumnName("idproblematica");
            e.Property(x => x.IdMagnitud).HasColumnName("idmagnitud");
            e.Property(x => x.IdGravedad).HasColumnName("idgravedad");
            e.Property(x => x.IdFrecuencia).HasColumnName("idfrecuencia");
            e.Property(x => x.NivelConfianza).HasColumnName("nivelconfianza").HasPrecision(5, 2);
            e.HasOne(x => x.Observacion).WithMany().HasForeignKey(x => x.IdObservacion).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Problematica).WithMany().HasForeignKey(x => x.IdProblematica).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Magnitud).WithMany().HasForeignKey(x => x.IdMagnitud).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Gravedad).WithMany().HasForeignKey(x => x.IdGravedad).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Frecuencia).WithMany().HasForeignKey(x => x.IdFrecuencia).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TendenciaProblematica>(e =>
        {
            e.ToTable("tendenciaproblematica", "ocn");
            e.HasKey(x => x.IdTendenciaProblematica);
            e.Property(x => x.IdTendenciaProblematica).HasColumnName("idtendenciaproblematica").ValueGeneratedOnAdd();
            e.Property(x => x.IdZona).HasColumnName("idzona");
            e.Property(x => x.IdProblematica).HasColumnName("idproblematica");
            e.Property(x => x.IdPeriodo).HasColumnName("idperiodo");
            e.Property(x => x.IdTendencia).HasColumnName("idtendencia");
            e.Property(x => x.FechaCalculo).HasColumnName("fechacalculo").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
            e.HasOne(x => x.Zona).WithMany().HasForeignKey(x => x.IdZona).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Problematica).WithMany().HasForeignKey(x => x.IdProblematica).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Periodo).WithMany().HasForeignKey(x => x.IdPeriodo).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Tendencia).WithMany().HasForeignKey(x => x.IdTendencia).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Necesidad>(e =>
        {
            e.ToTable("necesidad", "ocn");
            e.HasKey(x => x.IdNecesidad);
            e.Property(x => x.IdNecesidad).HasColumnName("idnecesidad").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(300).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<ProblematicaNecesidad>(e =>
        {
            e.ToTable("problematicanecesidad", "ocn");
            e.HasKey(x => new { x.IdProblematica, x.IdNecesidad });
            e.Property(x => x.IdProblematica).HasColumnName("idproblematica").ValueGeneratedNever();
            e.Property(x => x.IdNecesidad).HasColumnName("idnecesidad").ValueGeneratedNever();
            e.HasOne(x => x.Problematica).WithMany().HasForeignKey(x => x.IdProblematica).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Necesidad).WithMany().HasForeignKey(x => x.IdNecesidad).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatFactorRiesgo>(e =>
        {
            e.ToTable("catfactorriesgo", "ocn");
            e.HasKey(x => x.IdFactorRiesgo);
            e.Property(x => x.IdFactorRiesgo).HasColumnName("idfactorriesgo").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<ProblematicaFactorRiesgo>(e =>
        {
            e.ToTable("problematicafactorriesgo", "ocn");
            e.HasKey(x => new { x.IdProblematica, x.IdFactorRiesgo });
            e.Property(x => x.IdProblematica).HasColumnName("idproblematica").ValueGeneratedNever();
            e.Property(x => x.IdFactorRiesgo).HasColumnName("idfactorriesgo").ValueGeneratedNever();
            e.HasOne(x => x.Problematica).WithMany().HasForeignKey(x => x.IdProblematica).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.FactorRiesgo).WithMany().HasForeignKey(x => x.IdFactorRiesgo).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatFactorProtector>(e =>
        {
            e.ToTable("catfactorprotector", "ocn");
            e.HasKey(x => x.IdFactorProtector);
            e.Property(x => x.IdFactorProtector).HasColumnName("idfactorprotector").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<ProblematicaFactorProtector>(e =>
        {
            e.ToTable("problematicafactorprotector", "ocn");
            e.HasKey(x => new { x.IdProblematica, x.IdFactorProtector });
            e.Property(x => x.IdProblematica).HasColumnName("idproblematica").ValueGeneratedNever();
            e.Property(x => x.IdFactorProtector).HasColumnName("idfactorprotector").ValueGeneratedNever();
            e.HasOne(x => x.Problematica).WithMany().HasForeignKey(x => x.IdProblematica).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.FactorProtector).WithMany().HasForeignKey(x => x.IdFactorProtector).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatTipoRecurso>(e =>
        {
            e.ToTable("cattiporecurso", "ocn");
            e.HasKey(x => x.IdTipoRecurso);
            e.Property(x => x.IdTipoRecurso).HasColumnName("idtiporecurso").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<CatEstadoRecurso>(e =>
        {
            e.ToTable("catestadorecurso", "ocn");
            e.HasKey(x => x.IdEstadoRecurso);
            e.Property(x => x.IdEstadoRecurso).HasColumnName("idestadorecurso").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<CatCostoRecurso>(e =>
        {
            e.ToTable("catcostorecurso", "ocn");
            e.HasKey(x => x.IdCostoRecurso);
            e.Property(x => x.IdCostoRecurso).HasColumnName("idcostorecurso").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Recurso>(e =>
        {
            e.ToTable("recurso", "ocn");
            e.HasKey(x => x.IdRecurso);
            e.Property(x => x.IdRecurso).HasColumnName("idrecurso").ValueGeneratedOnAdd();
            e.Property(x => x.CodigoRecurso).HasColumnName("codigorecurso").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(200).IsUnicode(false);
            e.Property(x => x.IdTipoRecurso).HasColumnName("idtiporecurso");
            e.Property(x => x.IdOrganizacionResponsable).HasColumnName("idorganizacionresponsable");
            e.Property(x => x.IdEstadoRecurso).HasColumnName("idestadorecurso");
            e.Property(x => x.IdCostoRecurso).HasColumnName("idcostorecurso");
            e.Property(x => x.HorarioGeneral).HasColumnName("horariogeneral").HasMaxLength(200).IsUnicode(false);
            e.Property(x => x.CapacidadAproximada).HasColumnName("capacidadaproximada");
            e.Property(x => x.ConocimientoComunidad).HasColumnName("conocimientocomunidad").HasMaxLength(40).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
            e.HasOne(x => x.TipoRecurso).WithMany().HasForeignKey(x => x.IdTipoRecurso).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.OrganizacionResponsable).WithMany().HasForeignKey(x => x.IdOrganizacionResponsable).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.EstadoRecurso).WithMany().HasForeignKey(x => x.IdEstadoRecurso).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.CostoRecurso).WithMany().HasForeignKey(x => x.IdCostoRecurso).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<FactorProtectorRecurso>(e =>
        {
            e.ToTable("factorprotectorrecurso", "ocn");
            e.HasKey(x => new { x.IdFactorProtector, x.IdRecurso });
            e.Property(x => x.IdFactorProtector).HasColumnName("idfactorprotector").ValueGeneratedNever();
            e.Property(x => x.IdRecurso).HasColumnName("idrecurso").ValueGeneratedNever();
            e.HasOne(x => x.FactorProtector).WithMany().HasForeignKey(x => x.IdFactorProtector).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Recurso).WithMany().HasForeignKey(x => x.IdRecurso).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatServicio>(e =>
        {
            e.ToTable("catservicio", "ocn");
            e.HasKey(x => x.IdServicio);
            e.Property(x => x.IdServicio).HasColumnName("idservicio").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<RecursoServicio>(e =>
        {
            e.ToTable("recursoservicio", "ocn");
            e.HasKey(x => new { x.IdRecurso, x.IdServicio });
            e.Property(x => x.IdRecurso).HasColumnName("idrecurso").ValueGeneratedNever();
            e.Property(x => x.IdServicio).HasColumnName("idservicio").ValueGeneratedNever();
            e.HasOne(x => x.Recurso).WithMany().HasForeignKey(x => x.IdRecurso).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Servicio).WithMany().HasForeignKey(x => x.IdServicio).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ServicioNecesidad>(e =>
        {
            e.ToTable("servicionecesidad", "ocn");
            e.HasKey(x => new { x.IdServicio, x.IdNecesidad });
            e.Property(x => x.IdServicio).HasColumnName("idservicio").ValueGeneratedNever();
            e.Property(x => x.IdNecesidad).HasColumnName("idnecesidad").ValueGeneratedNever();
            e.HasOne(x => x.Servicio).WithMany().HasForeignKey(x => x.IdServicio).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Necesidad).WithMany().HasForeignKey(x => x.IdNecesidad).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatTipoCobertura>(e =>
        {
            e.ToTable("cattipocobertura", "ocn");
            e.HasKey(x => x.IdTipoCobertura);
            e.Property(x => x.IdTipoCobertura).HasColumnName("idtipocobertura").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<RecursoZonaCobertura>(e =>
        {
            e.ToTable("recursozonacobertura", "ocn");
            e.HasKey(x => x.IdRecursoZonaCobertura);
            e.Property(x => x.IdRecursoZonaCobertura).HasColumnName("idrecursozonacobertura").ValueGeneratedOnAdd();
            e.Property(x => x.IdRecurso).HasColumnName("idrecurso");
            e.Property(x => x.IdZona).HasColumnName("idzona");
            e.Property(x => x.IdTipoCobertura).HasColumnName("idtipocobertura");
            e.Property(x => x.FechaInicio).HasColumnName("fechainicio").HasColumnType("date");
            e.Property(x => x.FechaFin).HasColumnName("fechafin").HasColumnType("date");
            e.HasOne(x => x.Recurso).WithMany().HasForeignKey(x => x.IdRecurso).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Zona).WithMany().HasForeignKey(x => x.IdZona).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.TipoCobertura).WithMany().HasForeignKey(x => x.IdTipoCobertura).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatBarrera>(e =>
        {
            e.ToTable("catbarrera", "ocn");
            e.HasKey(x => x.IdBarrera);
            e.Property(x => x.IdBarrera).HasColumnName("idbarrera").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<RecursoZonaBarrera>(e =>
        {
            e.ToTable("recursozonabarrera", "ocn");
            e.HasKey(x => x.IdRecursoZonaBarrera);
            e.Property(x => x.IdRecursoZonaBarrera).HasColumnName("idrecursozonabarrera").ValueGeneratedOnAdd();
            e.Property(x => x.IdRecurso).HasColumnName("idrecurso");
            e.Property(x => x.IdZona).HasColumnName("idzona");
            e.Property(x => x.IdBarrera).HasColumnName("idbarrera");
            e.Property(x => x.IdGrupoPoblacional).HasColumnName("idgrupopoblacional");
            e.Property(x => x.FechaRegistro).HasColumnName("fecharegistro").HasColumnType("date").HasDefaultValueSql("cast(getdate() as date)");
            e.Property(x => x.Notas).HasColumnName("notas").HasMaxLength(300).IsUnicode(false);
            e.HasOne(x => x.Recurso).WithMany().HasForeignKey(x => x.IdRecurso).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Zona).WithMany().HasForeignKey(x => x.IdZona).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Barrera).WithMany().HasForeignKey(x => x.IdBarrera).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.GrupoPoblacional).WithMany().HasForeignKey(x => x.IdGrupoPoblacional).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RecursoZonaAccesibilidad>(e =>
        {
            e.ToTable("recursozonaaccesibilidad", "ocn");
            e.HasKey(x => x.IdRecursoZonaAccesibilidad);
            e.Property(x => x.IdRecursoZonaAccesibilidad).HasColumnName("idrecursozonaaccesibilidad").ValueGeneratedOnAdd();
            e.Property(x => x.IdRecurso).HasColumnName("idrecurso");
            e.Property(x => x.IdZona).HasColumnName("idzona");
            e.Property(x => x.NivelAccesibilidadPercibida).HasColumnName("nivelaccesibilidadpercibida").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.FechaEvaluacion).HasColumnName("fechaevaluacion").HasColumnType("date");
            e.HasOne(x => x.Recurso).WithMany().HasForeignKey(x => x.IdRecurso).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Zona).WithMany().HasForeignKey(x => x.IdZona).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatTipoEvidencia>(e =>
        {
            e.ToTable("cattipoevidencia", "ocn");
            e.HasKey(x => x.IdTipoEvidencia);
            e.Property(x => x.IdTipoEvidencia).HasColumnName("idtipoevidencia").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(60).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<Evidencia>(e =>
        {
            e.ToTable("evidencia", "ocn");
            e.HasKey(x => x.IdEvidencia);
            e.Property(x => x.IdEvidencia).HasColumnName("idevidencia").ValueGeneratedOnAdd();
            e.Property(x => x.IdObservacionProblematica).HasColumnName("idobservacionproblematica");
            e.Property(x => x.IdTipoEvidencia).HasColumnName("idtipoevidencia");
            e.Property(x => x.IdFuente).HasColumnName("idfuente");
            e.Property(x => x.NumeroMenciones).HasColumnName("numeromenciones");
            e.Property(x => x.DescripcionBreve).HasColumnName("descripcionbreve").HasMaxLength(300).IsUnicode(false);
            e.Property(x => x.FechaRegistro).HasColumnName("fecharegistro").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
            e.HasOne(x => x.ObservacionProblematica).WithMany().HasForeignKey(x => x.IdObservacionProblematica).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.TipoEvidencia).WithMany().HasForeignKey(x => x.IdTipoEvidencia).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Fuente).WithMany().HasForeignKey(x => x.IdFuente).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CatTipoIntervencion>(e =>
        {
            e.ToTable("cattipointervencion", "ocn");
            e.HasKey(x => x.IdTipoIntervencion);
            e.Property(x => x.IdTipoIntervencion).HasColumnName("idtipointervencion").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<NecesidadTipoIntervencion>(e =>
        {
            e.ToTable("necesidadtipointervencion", "ocn");
            e.HasKey(x => new { x.IdNecesidad, x.IdTipoIntervencion });
            e.Property(x => x.IdNecesidad).HasColumnName("idnecesidad").ValueGeneratedNever();
            e.Property(x => x.IdTipoIntervencion).HasColumnName("idtipointervencion").ValueGeneratedNever();
            e.HasOne(x => x.Necesidad).WithMany().HasForeignKey(x => x.IdNecesidad).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.TipoIntervencion).WithMany().HasForeignKey(x => x.IdTipoIntervencion).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TipoIntervencionServicio>(e =>
        {
            e.ToTable("tipointervencionservicio", "ocn");
            e.HasKey(x => new { x.IdTipoIntervencion, x.IdServicio });
            e.Property(x => x.IdTipoIntervencion).HasColumnName("idtipointervencion").ValueGeneratedNever();
            e.Property(x => x.IdServicio).HasColumnName("idservicio").ValueGeneratedNever();
            e.HasOne(x => x.TipoIntervencion).WithMany().HasForeignKey(x => x.IdTipoIntervencion).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Servicio).WithMany().HasForeignKey(x => x.IdServicio).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ModeloPriorizacion>(e =>
        {
            e.ToTable("modelopriorizacion", "ocn");
            e.HasKey(x => x.IdModelo);
            e.Property(x => x.IdModelo).HasColumnName("idmodelo").ValueGeneratedOnAdd();
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(false);
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(300).IsUnicode(false);
            e.Property(x => x.Activo).HasColumnName("activo");
            e.Property(x => x.FechaCreacion).HasColumnName("fechacreacion").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
        });

        modelBuilder.Entity<ModeloPonderacion>(e =>
        {
            e.ToTable("modeloponderacion", "ocn");
            e.HasKey(x => new { x.IdModelo, x.Variable });
            e.Property(x => x.IdModelo).HasColumnName("idmodelo").ValueGeneratedNever();
            e.Property(x => x.Variable).HasColumnName("variable").HasMaxLength(50).IsUnicode(false).ValueGeneratedNever();
            e.Property(x => x.Peso).HasColumnName("peso").HasPrecision(5, 2);
            e.HasOne(x => x.Modelo).WithMany().HasForeignKey(x => x.IdModelo).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PrioridadCalculada>(e =>
        {
            e.ToTable("prioridadcalculada", "ocn");
            e.HasKey(x => x.IdPrioridadCalculada);
            e.Property(x => x.IdPrioridadCalculada).HasColumnName("idprioridadcalculada").ValueGeneratedOnAdd();
            e.Property(x => x.IdZona).HasColumnName("idzona");
            e.Property(x => x.IdProblematica).HasColumnName("idproblematica");
            e.Property(x => x.IdPeriodo).HasColumnName("idperiodo");
            e.Property(x => x.IdGrupoPoblacional).HasColumnName("idgrupopoblacional");
            e.Property(x => x.IdModelo).HasColumnName("idmodelo");
            e.Property(x => x.ValorMagnitud).HasColumnName("valormagnitud").HasPrecision(6, 2);
            e.Property(x => x.ValorGravedad).HasColumnName("valorgravedad").HasPrecision(6, 2);
            e.Property(x => x.ValorFrecuencia).HasColumnName("valorfrecuencia").HasPrecision(6, 2);
            e.Property(x => x.ValorVulnerabilidad).HasColumnName("valorvulnerabilidad").HasPrecision(6, 2);
            e.Property(x => x.ValorEvidencia).HasColumnName("valorevidencia").HasPrecision(6, 2);
            e.Property(x => x.ValorTendencia).HasColumnName("valortendencia").HasPrecision(6, 2);
            e.Property(x => x.ValorBrecha).HasColumnName("valorbrecha").HasPrecision(6, 2);
            e.Property(x => x.PuntajeResultante).HasColumnName("puntajeresultante").HasPrecision(6, 2);
            e.Property(x => x.FechaCalculo).HasColumnName("fechacalculo").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
            e.HasOne(x => x.Zona).WithMany().HasForeignKey(x => x.IdZona).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Problematica).WithMany().HasForeignKey(x => x.IdProblematica).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Periodo).WithMany().HasForeignKey(x => x.IdPeriodo).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.GrupoPoblacional).WithMany().HasForeignKey(x => x.IdGrupoPoblacional).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Modelo).WithMany().HasForeignKey(x => x.IdModelo).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BrechaAtencion>(e =>
        {
            e.ToTable("brechaatencion", "ocn");
            e.HasKey(x => x.IdBrechaAtencion);
            e.Property(x => x.IdBrechaAtencion).HasColumnName("idbrechaatencion").ValueGeneratedOnAdd();
            e.Property(x => x.IdZona).HasColumnName("idzona");
            e.Property(x => x.IdNecesidad).HasColumnName("idnecesidad");
            e.Property(x => x.IdPeriodo).HasColumnName("idperiodo");
            e.Property(x => x.RecursosIdentificados).HasColumnName("recursosidentificados");
            e.Property(x => x.NivelBrecha).HasColumnName("nivelbrecha").HasMaxLength(30).IsUnicode(false);
            e.Property(x => x.FechaCalculo).HasColumnName("fechacalculo").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
            e.HasOne(x => x.Zona).WithMany().HasForeignKey(x => x.IdZona).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Necesidad).WithMany().HasForeignKey(x => x.IdNecesidad).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Periodo).WithMany().HasForeignKey(x => x.IdPeriodo).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Auditoria>(e =>
        {
            e.ToTable("auditoria", "ocn");
            e.HasKey(x => x.IdAuditoria);
            e.Property(x => x.IdAuditoria).HasColumnName("idauditoria").ValueGeneratedOnAdd();
            e.Property(x => x.NombreTabla).HasColumnName("nombretabla").HasMaxLength(100).IsUnicode(false);
            e.Property(x => x.IdRegistro).HasColumnName("idregistro");
            e.Property(x => x.Operacion).HasColumnName("operacion").HasMaxLength(10).IsUnicode(false);
            e.Property(x => x.IdUsuario).HasColumnName("idusuario");
            e.Property(x => x.FechaOperacion).HasColumnName("fechaoperacion").HasColumnType("datetime2").HasDefaultValueSql("sysdatetime()");
            e.Property(x => x.ValoresAnteriores).HasColumnName("valoresanteriores").HasColumnType("nvarchar(max)");
            e.Property(x => x.ValoresNuevos).HasColumnName("valoresnuevos").HasColumnType("nvarchar(max)");
            e.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.IdUsuario).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
