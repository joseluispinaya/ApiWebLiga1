using ApiWebLiga.Models;
using ApiWebLiga.Responses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ApiWebLiga.Data
{
    public class CategoriaData
    {
        public static Respuesta<List<Categoria>> ListaCategorias()
        {
            // 1. Iniciamos la respuesta por defecto en "Error" por si algo falla
            Respuesta<List<Categoria>> rpt = new Respuesta<List<Categoria>>()
            {
                Estado = false,
                Data = new List<Categoria>(), // Lista vacía, no nula
                Mensaje = "Error desconocido"
            };

            try
            {
                // Usamos la cadena limpia del Web.config
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ListaCategorias", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new Categoria
                                {
                                    IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                    NombreCategoria = dr["NombreCategoria"].ToString(),
                                    Genero = Convert.ToChar(dr["Genero"].ToString()),
                                    EdadMaxima = dr["EdadMaxima"] != DBNull.Value ? Convert.ToInt32(dr["EdadMaxima"]) : 0,
                                    Estado = Convert.ToBoolean(dr["Estado"])
                                });
                            }
                        }
                    }
                }

                // Si todo salió bien, actualizamos la respuesta
                rpt.Estado = true;
                rpt.Mensaje = "Lista obtenida correctamente";
            }
            catch (Exception ex)
            {
                // Si hay error, el frontend sabrá exactamente qué pasó
                rpt.Estado = false;
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }

    }
}