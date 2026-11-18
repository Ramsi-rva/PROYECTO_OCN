using System.Data;
using Microsoft.Data.SqlClient;
using OcnMvc.Models;

namespace OcnMvc.Datos;

// Capa de datos de ocn.registroocn: todo el código que habla con SQL Server.
// Cada método ejecuta un procedimiento almacenado.
public class RegistroOCNDatos
{
    private readonly string _cadenaConexion;

    public RegistroOCNDatos(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    // READ: lista de registros activos
    public List<RegistroOCN> Listar()
    {
        var registros = new List<RegistroOCN>();

        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_registroocn_listar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            registros.Add(Mapear(lector));
        }

        return registros;
    }

    // READ: un registro por id
    // Devuelve null si no existe.
    public RegistroOCN? Buscar(int idRegistroOCN)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_registroocn_buscar", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@idregistroocn", SqlDbType.Int).Value = idRegistroOCN;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        return lector.Read() ? Mapear(lector) : null;
    }

    // CREATE
    // Devuelve el id que generó SQL Server (parámetro de salida).
    public int Insertar(RegistroOCN registro)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_registroocn_insertar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        AgregarParametrosComunes(comando, registro);

        var parametroId = comando.Parameters.Add("@idregistroocn", SqlDbType.Int);
        parametroId.Direction = ParameterDirection.Output;

        conexion.Open();
        comando.ExecuteNonQuery();

        return (int)parametroId.Value;
    }

    // UPDATE
    public void Actualizar(RegistroOCN registro)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_registroocn_actualizar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        comando.Parameters.Add("@idregistroocn", SqlDbType.Int).Value = registro.IdRegistroOCN;
        AgregarParametrosComunes(comando, registro);

        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // DELETE (lógico)
    public void Eliminar(int idRegistroOCN)
    {
        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_registroocn_eliminar", conexion);
        comando.CommandType = CommandType.StoredProcedure;
        comando.Parameters.Add("@idregistroocn", SqlDbType.Int).Value = idRegistroOCN;

        conexion.Open();
        comando.ExecuteNonQuery();
    }

    // Métodos auxiliares (privados: solo se usan dentro de esta clase)

    // Parámetros que comparten insertar y actualizar.
    private static void AgregarParametrosComunes(SqlCommand comando, RegistroOCN registro)
    {
        comando.Parameters.Add("@idzona", SqlDbType.Int).Value = registro.IdZona;
        comando.Parameters.Add("@idproblematica", SqlDbType.Int).Value = registro.IdProblematica;
        comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 500).Value =
            (object?)registro.Descripcion ?? DBNull.Value;
        comando.Parameters.Add("@intensidad", SqlDbType.TinyInt).Value = registro.Intensidad;
        comando.Parameters.Add("@frecuencia", SqlDbType.VarChar, 20).Value = registro.Frecuencia;
        comando.Parameters.Add("@fecharegistro", SqlDbType.DateTime2).Value = registro.FechaRegistro;
    }

    // Convierte la fila actual del lector en un objeto RegistroOCN.
    // Los nombres de la zona y la problemática vienen del join del procedimiento.
    private static RegistroOCN Mapear(SqlDataReader lector)
    {
        int idZona = lector.GetInt32(lector.GetOrdinal("idzona"));
        int idProblematica = lector.GetInt32(lector.GetOrdinal("idproblematica"));

        return new RegistroOCN
        {
            IdRegistroOCN  = lector.GetInt32(lector.GetOrdinal("idregistroocn")),
            IdZona         = idZona,
            IdProblematica = idProblematica,
            Descripcion    = lector.IsDBNull(lector.GetOrdinal("descripcion"))
                                 ? null
                                 : lector.GetString(lector.GetOrdinal("descripcion")),
            Intensidad     = lector.GetByte(lector.GetOrdinal("intensidad")),
            Frecuencia     = lector.GetString(lector.GetOrdinal("frecuencia")),
            FechaRegistro  = lector.GetDateTime(lector.GetOrdinal("fecharegistro")),
            Activo         = lector.GetBoolean(lector.GetOrdinal("activo")),
            Zona = new Zona
            {
                IdZona = idZona,
                Nombre = lector.GetString(lector.GetOrdinal("nombrezona"))
            },
            Problematica = new Problematica
            {
                IdProblematica = idProblematica,
                Nombre         = lector.GetString(lector.GetOrdinal("nombreproblematica"))
            }
        };
    }
}
