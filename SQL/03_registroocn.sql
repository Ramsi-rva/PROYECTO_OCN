use proyecto_ocn;
go

-- tabla RegistroOCN: relaciona una zona con una problemática
create table ocn.registroocn (
    idregistroocn   int identity(1,1) primary key,
    idzona          int not null foreign key references ocn.zona(idzona),
    idproblematica  int not null foreign key references ocn.problematica(idproblematica),
    descripcion     varchar(500) null,
    fecharegistro   datetime2 not null constraint df_registroocn_fecha default (sysdatetime()),
    activo          bit not null constraint df_registroocn_activo default (1)
);
go
create index ix_registroocn_zona on ocn.registroocn(idzona);
create index ix_registroocn_problematica on ocn.registroocn(idproblematica);
go

-- datos de ejemplo
insert into ocn.registroocn (idzona, idproblematica, descripcion)
select top 5 z.idzona, p.idproblematica, 'registro de ejemplo'
from ocn.zona z
cross join ocn.problematica p
where p.idproblematica <= 3
order by z.idzona, p.idproblematica;
go

select * from ocn.registroocn;
go
