using System.Data;
using Microsoft.Data.SqlClient;
using OcnMvc.Models;

namespace OcnMvc.Datos;

// Capa de datos de los catálogos (solo lectura).
// Se usan para llenar las listas desplegables de los formularios.
public class CatalogoDatos
{
    private readonly string _cadenaConexion;

    public CatalogoDatos(string cadenaConexion)
    {
        _cadenaConexion = cadenaConexion;
    }

    // READ: divisiones territoriales activas
    public List<DivisionTerritorial> ListarDivisiones()
    {
        var divisiones = new List<DivisionTerritorial>();

        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_divisionterritorial_listar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            divisiones.Add(new DivisionTerritorial
            {
                IdDivision = lector.GetInt32(lector.GetOrdinal("iddivision")),
                Nombre     = lector.GetString(lector.GetOrdinal("nombre")),
                Tipo       = lector.IsDBNull(lector.GetOrdinal("tipo"))
                                 ? null
                                 : lector.GetString(lector.GetOrdinal("tipo")),
                Activo     = lector.GetBoolean(lector.GetOrdinal("activo"))
            });
        }

        return divisiones;
    }

    // READ: tipos de zona activos
    public List<CatTipoZona> ListarTiposZona()
    {
        var tipos = new List<CatTipoZona>();

        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_cattipozona_listar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            tipos.Add(new CatTipoZona
            {
                IdTipoZona = lector.GetInt32(lector.GetOrdinal("idtipozona")),
                Nombre     = lector.GetString(lector.GetOrdinal("nombre")),
                Activo     = lector.GetBoolean(lector.GetOrdinal("activo"))
            });
        }

        return tipos;
    }

    // READ: categorías de problemática activas
    public List<CategoriaProblematica> ListarCategorias()
    {
        var categorias = new List<CategoriaProblematica>();

        using var conexion = new SqlConnection(_cadenaConexion);
        using var comando = new SqlCommand("ocn.pa_categoriaproblematica_listar", conexion);
        comando.CommandType = CommandType.StoredProcedure;

        conexion.Open();
        using var lector = comando.ExecuteReader();

        while (lector.Read())
        {
            categorias.Add(new CategoriaProblematica
            {
                IdCategoria = lector.GetInt32(lector.GetOrdinal("idcategoria")),
                Nombre      = lector.GetString(lector.GetOrdinal("nombre")),
                Activo      = lector.GetBoolean(lector.GetOrdinal("activo"))
            });
        }

        return categorias;
    }
}
