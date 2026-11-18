using System.Data;
using Microsoft.Data.SqlClient;
using OcnMvc.Models;

namespace OcnMvc.Datos;

// Capa de datos de ocn.problematica: todo el código que habla con SQL Server.
// Cada método ejecuta un procedimiento almacenado.
public class ProblematicaDatos
{
    private readonly string _cadenaConexion;

    public ProblematicaDatos(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    // READ: lista de problemáticas activas
    public List<Problematica> Listar()
    {
        var problematicas = new List<Problematica>();

        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_problematica_listar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            problematicas.Add(Mapear(lector));
        }

        return problematicas;
    }

    // READ: una problemática por id
    // Devuelve null si no existe.
    public Problematica? Buscar(int idProblematica)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_problematica_buscar", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@idproblematica", SqlDbType.Int).Value = idProblematica;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        return lector.Read() ? Mapear(lector) : null;
    }

    // CREATE
    // Devuelve el id que generó SQL Server (parámetro de salida).
    public int Insertar(Problematica problematica)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_problematica_insertar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        AgregarParametrosComunes(comando, problematica);

        var parametroId = comando.Parameters.Add("@idproblematica", SqlDbType.Int);
        parametroId.Direction = ParameterDirection.Output;

        conexion.Open();
        comando.ExecuteNonQuery();

        return (int)parametroId.Value;
    }

    // UPDATE
    public void Actualizar(Problematica problematica)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_problematica_actualizar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@idproblematica", SqlDbType.Int).Value = problematica.IdProblematica;
        AgregarParametrosComunes(comando, problematica);

        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // DELETE (lógico)
    public void Eliminar(int idProblematica)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_problematica_eliminar", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@idproblematica", SqlDbType.Int).Value = idProblematica;

        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // Métodos auxiliares (privados: solo se usan dentro de esta clase)

    // Parámetros que comparten insertar y actualizar.
    private static void AgregarParametrosComunes(SqlCommand comando, Problematica problematica)
    {
        comando.Parameters.Add("@idcategoria", SqlDbType.Int).Value = problematica.IdCategoria;

        // La problemática padre es opcional: null de C# → NULL de SQL.
        comando.Parameters.Add("@idproblematicapadre", SqlDbType.Int).Value =
            (object?)problematica.IdProblematicaPadre ?? DBNull.Value;

        comando.Parameters.Add("@nombre", SqlDbType.VarChar, 200).Value = problematica.Nombre;
        comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 500).Value =
            (object?)problematica.Descripcion ?? DBNull.Value;
    }

    // Convierte la fila actual del lector en un objeto Problematica.
    // Los nombres de la categoría y del padre vienen del join del procedimiento.
    private static Problematica Mapear(SqlDataReader lector)
    {
        int idCategoria = lector.GetInt32(lector.GetOrdinal("idcategoria"));
        int? idPadre = lector.IsDBNull(lector.GetOrdinal("idproblematicapadre"))
                           ? null
                           : lector.GetInt32(lector.GetOrdinal("idproblematicapadre"));

        return new Problematica
        {
            IdProblematica      = lector.GetInt32(lector.GetOrdinal("idproblematica")),
            IdCategoria         = idCategoria,
            IdProblematicaPadre = idPadre,
            Nombre              = lector.GetString(lector.GetOrdinal("nombre")),
            Descripcion         = lector.IsDBNull(lector.GetOrdinal("descripcion"))
                                      ? null
                                      : lector.GetString(lector.GetOrdinal("descripcion")),
            Activo              = lector.GetBoolean(lector.GetOrdinal("activo")),
            Categoria = new CategoriaProblematica
            {
                IdCategoria = idCategoria,
                Nombre      = lector.GetString(lector.GetOrdinal("nombrecategoria"))
            },
            // Solo se arma el padre si la problemática tiene uno.
            ProblematicaPadre = idPadre is null
                ? null
                : new Problematica
                {
                    IdProblematica = idPadre.Value,
                    Nombre         = lector.GetString(lector.GetOrdinal("nombreproblematicapadre"))
                }
        };
    }
}
