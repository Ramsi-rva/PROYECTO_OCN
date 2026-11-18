use proyecto_ocn;
go

-- Procedimientos almacenados para zona, problematica y registroocn
-- (más los listados de catálogos que llenan las listas desplegables).
--
-- Códigos de error (throw) que la aplicación muestra en el formulario:
--   50001  valor repetido (código o nombre)
--   50002  el registro no existe o está inactivo
--   50003  la llave foránea apunta a un registro inexistente o inactivo
--   50004  no se puede dar de baja porque otros registros activos lo usan


-- =========================================================
-- CATÁLOGOS (solo lectura, para las listas desplegables)
-- =========================================================

-- 1. listar divisiones territoriales activas
create or alter procedure ocn.pa_divisionterritorial_listar
as
begin
    set nocount on;

    select iddivision, nombre, tipo, activo
    from ocn.divisionterritorial
    where activo = 1
    order by nombre;
end;
go

-- 2. listar tipos de zona activos
create or alter procedure ocn.pa_cattipozona_listar
as
begin
    set nocount on;

    select idtipozona, nombre, activo
    from ocn.cattipozona
    where activo = 1
    order by nombre;
end;
go

-- 3. listar categorías de problemática activas
create or alter procedure ocn.pa_categoriaproblematica_listar
as
begin
    set nocount on;

    select idcategoria, nombre, activo
    from ocn.categoriaproblematica
    where activo = 1
    order by nombre;
end;
go


-- =========================================================
-- ZONA
-- =========================================================

-- 1. listar (read): zonas activas con el nombre de su división y tipo
create or alter procedure ocn.pa_zona_listar
as
begin
    set nocount on;

    select z.idzona, z.codigozona, z.nombre, z.iddivision, z.idtipozona,
           z.descripciongeneral, z.referenciageograficaaprox, z.activo,
           d.nombre as nombredivision,
           t.nombre as nombretipozona
    from ocn.zona z
    inner join ocn.divisionterritorial d on d.iddivision = z.iddivision
    inner join ocn.cattipozona t on t.idtipozona = z.idtipozona
    where z.activo = 1
    order by z.nombre;
end;
go

-- 2. buscar (read): una zona activa por su id
create or alter procedure ocn.pa_zona_buscar
    @idzona int
as
begin
    set nocount on;

    select z.idzona, z.codigozona, z.nombre, z.iddivision, z.idtipozona,
           z.descripciongeneral, z.referenciageograficaaprox, z.activo,
           d.nombre as nombredivision,
           t.nombre as nombretipozona
    from ocn.zona z
    inner join ocn.divisionterritorial d on d.iddivision = z.iddivision
    inner join ocn.cattipozona t on t.idtipozona = z.idtipozona
    where z.idzona = @idzona
      and z.activo = 1;
end;
go

-- 3. insertar (create): devuelve el id generado en un parámetro de salida
create or alter procedure ocn.pa_zona_insertar
    @codigozona                 varchar(30),
    @nombre                     varchar(150),
    @iddivision                 int,
    @idtipozona                 int,
    @descripciongeneral         varchar(500) = null,
    @referenciageograficaaprox  varchar(200) = null,
    @idzona                     int output
as
begin
    set nocount on;

    if exists (select 1 from ocn.zona where codigozona = @codigozona)
        throw 50001, 'Ya existe una zona con ese código.', 1;

    if not exists (select 1 from ocn.divisionterritorial where iddivision = @iddivision and activo = 1)
        throw 50003, 'La división territorial no existe o está inactiva.', 1;

    if not exists (select 1 from ocn.cattipozona where idtipozona = @idtipozona and activo = 1)
        throw 50003, 'El tipo de zona no existe o está inactivo.', 1;

    insert into ocn.zona (codigozona, nombre, iddivision, idtipozona,
                          descripciongeneral, referenciageograficaaprox, activo)
    values (@codigozona, @nombre, @iddivision, @idtipozona,
            @descripciongeneral, @referenciageograficaaprox, 1);

    set @idzona = scope_identity();
end;
go

-- 4. actualizar (update)
create or alter procedure ocn.pa_zona_actualizar
    @idzona                     int,
    @codigozona                 varchar(30),
    @nombre                     varchar(150),
    @iddivision                 int,
    @idtipozona                 int,
    @descripciongeneral         varchar(500) = null,
    @referenciageograficaaprox  varchar(200) = null
as
begin
    set nocount on;

    if not exists (select 1 from ocn.zona where idzona = @idzona and activo = 1)
        throw 50002, 'No existe una zona activa con ese id.', 1;

    if exists (select 1 from ocn.zona where codigozona = @codigozona and idzona <> @idzona)
        throw 50001, 'Ya existe otra zona con ese código.', 1;

    if not exists (select 1 from ocn.divisionterritorial where iddivision = @iddivision and activo = 1)
        throw 50003, 'La división territorial no existe o está inactiva.', 1;

    if not exists (select 1 from ocn.cattipozona where idtipozona = @idtipozona and activo = 1)
        throw 50003, 'El tipo de zona no existe o está inactivo.', 1;

    update ocn.zona
    set codigozona                = @codigozona,
        nombre                    = @nombre,
        iddivision                = @iddivision,
        idtipozona                = @idtipozona,
        descripciongeneral        = @descripciongeneral,
        referenciageograficaaprox = @referenciageograficaaprox
    where idzona = @idzona;
end;
go

-- 5. eliminar (delete): borrado lógico, solo cambia activo a 0
create or alter procedure ocn.pa_zona_eliminar
    @idzona int
as
begin
    set nocount on;

    if not exists (select 1 from ocn.zona where idzona = @idzona and activo = 1)
        throw 50002, 'No existe una zona activa con ese id.', 1;

    if exists (select 1 from ocn.registroocn where idzona = @idzona and activo = 1)
        throw 50004, 'No se puede dar de baja: la zona tiene registros OCN activos.', 1;

    update ocn.zona
    set activo = 0
    where idzona = @idzona;
end;
go


-- =========================================================
-- PROBLEMATICA
-- =========================================================

-- 1. listar (read): problemáticas activas con su categoría y su padre (si tiene)
create or alter procedure ocn.pa_problematica_listar
as
begin
    set nocount on;

    select p.idproblematica, p.idcategoria, p.idproblematicapadre,
           p.nombre, p.descripcion, p.activo,
           c.nombre  as nombrecategoria,
           pp.nombre as nombreproblematicapadre
    from ocn.problematica p
    inner join ocn.categoriaproblematica c on c.idcategoria = p.idcategoria
    left join ocn.problematica pp on pp.idproblematica = p.idproblematicapadre
    where p.activo = 1
    order by p.nombre;
end;
go

-- 2. buscar (read): una problemática activa por su id
create or alter procedure ocn.pa_problematica_buscar
    @idproblematica int
as
begin
    set nocount on;

    select p.idproblematica, p.idcategoria, p.idproblematicapadre,
           p.nombre, p.descripcion, p.activo,
           c.nombre  as nombrecategoria,
           pp.nombre as nombreproblematicapadre
    from ocn.problematica p
    inner join ocn.categoriaproblematica c on c.idcategoria = p.idcategoria
    left join ocn.problematica pp on pp.idproblematica = p.idproblematicapadre
    where p.idproblematica = @idproblematica
      and p.activo = 1;
end;
go

-- 3. insertar (create): devuelve el id generado en un parámetro de salida
create or alter procedure ocn.pa_problematica_insertar
    @idcategoria          int,
    @idproblematicapadre  int = null,
    @nombre               varchar(200),
    @descripcion          varchar(500) = null,
    @idproblematica       int output
as
begin
    set nocount on;

    if not exists (select 1 from ocn.categoriaproblematica where idcategoria = @idcategoria and activo = 1)
        throw 50003, 'La categoría no existe o está inactiva.', 1;

    if @idproblematicapadre is not null
       and not exists (select 1 from ocn.problematica where idproblematica = @idproblematicapadre and activo = 1)
        throw 50003, 'La problemática padre no existe o está inactiva.', 1;

    if exists (select 1 from ocn.problematica where nombre = @nombre and idcategoria = @idcategoria and activo = 1)
        throw 50001, 'Ya existe una problemática con ese nombre en la categoría.', 1;

    insert into ocn.problematica (idcategoria, idproblematicapadre, nombre, descripcion, activo)
    values (@idcategoria, @idproblematicapadre, @nombre, @descripcion, 1);

    set @idproblematica = scope_identity();
end;
go

-- 4. actualizar (update)
create or alter procedure ocn.pa_problematica_actualizar
    @idproblematica       int,
    @idcategoria          int,
    @idproblematicapadre  int = null,
    @nombre               varchar(200),
    @descripcion          varchar(500) = null
as
begin
    set nocount on;

    if not exists (select 1 from ocn.problematica where idproblematica = @idproblematica and activo = 1)
        throw 50002, 'No existe una problemática activa con ese id.', 1;

    if not exists (select 1 from ocn.categoriaproblematica where idcategoria = @idcategoria and activo = 1)
        throw 50003, 'La categoría no existe o está inactiva.', 1;

    if @idproblematicapadre = @idproblematica
        throw 50003, 'Una problemática no puede ser su propia problemática padre.', 1;

    if @idproblematicapadre is not null
       and not exists (select 1 from ocn.problematica where idproblematica = @idproblematicapadre and activo = 1)
        throw 50003, 'La problemática padre no existe o está inactiva.', 1;

    if exists (select 1 from ocn.problematica
               where nombre = @nombre and idcategoria = @idcategoria
                 and activo = 1 and idproblematica <> @idproblematica)
        throw 50001, 'Ya existe otra problemática con ese nombre en la categoría.', 1;

    update ocn.problematica
    set idcategoria         = @idcategoria,
        idproblematicapadre = @idproblematicapadre,
        nombre              = @nombre,
        descripcion         = @descripcion
    where idproblematica = @idproblematica;
end;
go

-- 5. eliminar (delete): borrado lógico, solo cambia activo a 0
create or alter procedure ocn.pa_problematica_eliminar
    @idproblematica int
as
begin
    set nocount on;

    if not exists (select 1 from ocn.problematica where idproblematica = @idproblematica and activo = 1)
        throw 50002, 'No existe una problemática activa con ese id.', 1;

    if exists (select 1 from ocn.problematica where idproblematicapadre = @idproblematica and activo = 1)
        throw 50004, 'No se puede dar de baja: otras problemáticas activas dependen de ella.', 1;

    if exists (select 1 from ocn.registroocn where idproblematica = @idproblematica and activo = 1)
        throw 50004, 'No se puede dar de baja: la problemática tiene registros OCN activos.', 1;

    update ocn.problematica
    set activo = 0
    where idproblematica = @idproblematica;
end;
go


-- =========================================================
-- REGISTROOCN
-- =========================================================

-- 1. listar (read): registros activos con el nombre de la zona y la problemática
create or alter procedure ocn.pa_registroocn_listar
as
begin
    set nocount on;

    select r.idregistroocn, r.idzona, r.idproblematica,
           r.descripcion, r.intensidad, r.frecuencia, r.fecharegistro, r.activo,
           z.nombre as nombrezona,
           p.nombre as nombreproblematica
    from ocn.registroocn r
    inner join ocn.zona z on z.idzona = r.idzona
    inner join ocn.problematica p on p.idproblematica = r.idproblematica
    where r.activo = 1
    order by r.fecharegistro desc;
end;
go

-- 2. buscar (read): un registro activo por su id
create or alter procedure ocn.pa_registroocn_buscar
    @idregistroocn int
as
begin
    set nocount on;

    select r.idregistroocn, r.idzona, r.idproblematica,
           r.descripcion, r.intensidad, r.frecuencia, r.fecharegistro, r.activo,
           z.nombre as nombrezona,
           p.nombre as nombreproblematica
    from ocn.registroocn r
    inner join ocn.zona z on z.idzona = r.idzona
    inner join ocn.problematica p on p.idproblematica = r.idproblematica
    where r.idregistroocn = @idregistroocn
      and r.activo = 1;
end;
go

-- 3. insertar (create): si no se manda fecha, se usa la del servidor
create or alter procedure ocn.pa_registroocn_insertar
    @idzona          int,
    @idproblematica  int,
    @descripcion     varchar(500) = null,
    @intensidad      tinyint = 1,
    @frecuencia      varchar(20) = 'Ocasional',
    @fecharegistro   datetime2 = null,
    @idregistroocn   int output
as
begin
    set nocount on;

    if not exists (select 1 from ocn.zona where idzona = @idzona and activo = 1)
        throw 50003, 'La zona no existe o está inactiva.', 1;

    if not exists (select 1 from ocn.problematica where idproblematica = @idproblematica and activo = 1)
        throw 50003, 'La problemática no existe o está inactiva.', 1;

    insert into ocn.registroocn (idzona, idproblematica, descripcion, intensidad, frecuencia, fecharegistro, activo)
    values (@idzona, @idproblematica, @descripcion, @intensidad, @frecuencia, isnull(@fecharegistro, sysdatetime()), 1);

    set @idregistroocn = scope_identity();
end;
go

-- 4. actualizar (update)
create or alter procedure ocn.pa_registroocn_actualizar
    @idregistroocn   int,
    @idzona          int,
    @idproblematica  int,
    @descripcion     varchar(500) = null,
    @intensidad      tinyint,
    @frecuencia      varchar(20),
    @fecharegistro   datetime2
as
begin
    set nocount on;

    if not exists (select 1 from ocn.registroocn where idregistroocn = @idregistroocn and activo = 1)
        throw 50002, 'No existe un registro OCN activo con ese id.', 1;

    if not exists (select 1 from ocn.zona where idzona = @idzona and activo = 1)
        throw 50003, 'La zona no existe o está inactiva.', 1;

    if not exists (select 1 from ocn.problematica where idproblematica = @idproblematica and activo = 1)
        throw 50003, 'La problemática no existe o está inactiva.', 1;

    update ocn.registroocn
    set idzona         = @idzona,
        idproblematica = @idproblematica,
        descripcion    = @descripcion,
        intensidad     = @intensidad,
        frecuencia     = @frecuencia,
        fecharegistro  = @fecharegistro
    where idregistroocn = @idregistroocn;
end;
go

-- 5. eliminar (delete): borrado lógico, solo cambia activo a 0
create or alter procedure ocn.pa_registroocn_eliminar
    @idregistroocn int
as
begin
    set nocount on;

    if not exists (select 1 from ocn.registroocn where idregistroocn = @idregistroocn and activo = 1)
        throw 50002, 'No existe un registro OCN activo con ese id.', 1;

    update ocn.registroocn
    set activo = 0
    where idregistroocn = @idregistroocn;
end;
go


-- prueba de los procedimientos almacenados
-- (comentadas: el insertar crea un registro real cada vez que se ejecuta el script)
-- exec ocn.pa_zona_listar;
-- exec ocn.pa_problematica_listar;
-- exec ocn.pa_registroocn_listar;
-- exec ocn.pa_registroocn_buscar @idregistroocn = 1;
--
-- declare @nuevo int;
-- exec ocn.pa_registroocn_insertar @idzona = 1, @idproblematica = 1,
--      @descripcion = 'prueba', @intensidad = 3, @frecuencia = 'Frecuente',
--      @idregistroocn = @nuevo output;
-- select @nuevo as id_generado;
--
-- select * from ocn.registroocn;
