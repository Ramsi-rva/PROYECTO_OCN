using System.Data;
using Microsoft.Data.SqlClient;
using OcnMvc.Models;

namespace OcnMvc.Datos;

// Capa de datos de ocn.zona: todo el código que habla con SQL Server.
// Cada método ejecuta un procedimiento almacenado.
public class ZonaDatos
{
    private readonly string _cadenaConexion;

    public ZonaDatos(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    // READ: lista de zonas activas
    public List<Zona> Listar()
    {
        var zonas = new List<Zona>();

        // "using" cierra la conexión automáticamente al terminar el método.
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_zona_listar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        // Read() avanza fila por fila; devuelve false cuando ya no hay más.
        while (lector.Read())
        {
            zonas.Add(Mapear(lector));
        }

        return zonas;
    }

    // READ: una zona por id
    // Devuelve null si no existe.
    public Zona? Buscar(int idZona)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_zona_buscar", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@idzona", SqlDbType.Int).Value = idZona;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        return lector.Read() ? Mapear(lector) : null;
    }

    // CREATE
    // Devuelve el id que generó SQL Server (parámetro de salida).
    public int Insertar(Zona zona)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_zona_insertar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        AgregarParametrosComunes(comando, zona);

        // Parámetro de salida: SQL Server escribe aquí el nuevo id.
        var parametroId = comando.Parameters.Add("@idzona", SqlDbType.Int);
        parametroId.Direction = ParameterDirection.Output;

        conexion.Open();
        comando.ExecuteNonQuery();

        return (int)parametroId.Value;
    }

    // UPDATE
    public void Actualizar(Zona zona)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_zona_actualizar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@idzona", SqlDbType.Int).Value = zona.IdZona;
        AgregarParametrosComunes(comando, zona);

        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // DELETE (lógico)
    public void Eliminar(int idZona)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_zona_eliminar", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@idzona", SqlDbType.Int).Value = idZona;

        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // Métodos auxiliares (privados: solo se usan dentro de esta clase)

    // Parámetros que comparten insertar y actualizar.
    private static void AgregarParametrosComunes(SqlCommand comando, Zona zona)
    {
        comando.Parameters.Add("@codigozona", SqlDbType.VarChar, 30).Value = zona.CodigoZona;
        comando.Parameters.Add("@nombre", SqlDbType.VarChar, 150).Value = zona.Nombre;
        comando.Parameters.Add("@iddivision", SqlDbType.Int).Value = zona.IdDivision;
        comando.Parameters.Add("@idtipozona", SqlDbType.Int).Value = zona.IdTipoZona;

        // Un null de C# no es lo mismo que un NULL de SQL: se envía DBNull.Value.
        comando.Parameters.Add("@descripciongeneral", SqlDbType.VarChar, 500).Value =
            (object?)zona.DescripcionGeneral ?? DBNull.Value;
        comando.Parameters.Add("@referenciageograficaaprox", SqlDbType.VarChar, 200).Value =
            (object?)zona.ReferenciaGeograficaAprox ?? DBNull.Value;
    }

    // Convierte la fila actual del lector en un objeto Zona.
    // Los nombres de la división y el tipo vienen del join del procedimiento.
    private static Zona Mapear(SqlDataReader lector)
    {
        int idDivision = lector.GetInt32(lector.GetOrdinal("iddivision"));
        int idTipoZona = lector.GetInt32(lector.GetOrdinal("idtipozona"));

        return new Zona
        {
            IdZona                    = lector.GetInt32(lector.GetOrdinal("idzona")),
            CodigoZona                = lector.GetString(lector.GetOrdinal("codigozona")),
            Nombre                    = lector.GetString(lector.GetOrdinal("nombre")),
            IdDivision                = idDivision,
            IdTipoZona                = idTipoZona,
            DescripcionGeneral        = lector.IsDBNull(lector.GetOrdinal("descripciongeneral"))
                                            ? null
                                            : lector.GetString(lector.GetOrdinal("descripciongeneral")),
            ReferenciaGeograficaAprox = lector.IsDBNull(lector.GetOrdinal("referenciageograficaaprox"))
                                            ? null
                                            : lector.GetString(lector.GetOrdinal("referenciageograficaaprox")),
            Activo                    = lector.GetBoolean(lector.GetOrdinal("activo")),
            Division = new DivisionTerritorial
            {
                IdDivision = idDivision,
                Nombre     = lector.GetString(lector.GetOrdinal("nombredivision"))
            },
            TipoZona = new CatTipoZona
            {
                IdTipoZona = idTipoZona,
                Nombre     = lector.GetString(lector.GetOrdinal("nombretipozona"))
            }
        };
    }
}
