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
    public class PartidosCatData
    {
        public static Respuesta<List<CategoriaConPartidosDTO>> ListaCategoriasConPartidos()
        {
            Respuesta<List<CategoriaConPartidosDTO>> rpt = new Respuesta<List<CategoriaConPartidosDTO>>()
            {
                Estado = false,
                Data = new List<CategoriaConPartidosDTO>(),
                Mensaje = "Error desconocido"
            };

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerCategoriasConPartidos", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new CategoriaConPartidosDTO
                                {
                                    IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                    NombreCategoria = dr["NombreCategoria"].ToString(),
                                    Genero = dr["Genero"].ToString(),

                                    // Validación segura para el NULL de la edad máxima
                                    EdadMaxima = dr["EdadMaxima"] != DBNull.Value ? (int?)Convert.ToInt32(dr["EdadMaxima"]) : null,

                                    CantidadPartidosProgramados = Convert.ToInt32(dr["CantidadPartidosProgramados"])
                                });
                            }
                        }
                    }
                }
                rpt.Estado = true;
                rpt.Mensaje = "Categorías y conteo de partidos obtenidos correctamente";
            }
            catch (Exception ex)
            {
                rpt.Estado = false;
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }

        public static Respuesta<List<PartidosDTO>> ListaPartidosCategoria(int idCategoria)
        {
            // 1. Iniciamos la respuesta por defecto en "Error" por si algo falla
            Respuesta<List<PartidosDTO>> rpt = new Respuesta<List<PartidosDTO>>()
            {
                Estado = false,
                Data = new List<PartidosDTO>(), // Lista vacía, no nula
                Mensaje = "Error desconocido"
            };

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerPartidosProgramadosApp", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;

                        // NUEVO: Agregamos el parámetro que ahora espera tu SP
                        comando.Parameters.AddWithValue("@IdCategoria", idCategoria);

                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new PartidosDTO
                                {
                                    IdPartido = Convert.ToInt32(dr["IdPartido"]),
                                    Fecha = Convert.ToDateTime(dr["Fecha"]).ToString("dd/MM/yyyy"),
                                    Hora = ((TimeSpan)dr["Hora"]).ToString(@"hh\:mm"),
                                    Cancha = dr["Cancha"].ToString(),
                                    ClubLocal = dr["ClubLocal"].ToString(),
                                    LogoLocal = dr["LogoLocal"].ToString(),
                                    ClubVisitante = dr["ClubVisitante"].ToString(),
                                    LogoVisitante = dr["LogoVisitante"].ToString()
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

        public static Respuesta<bool> GuardarConvocatoria(ConvocatoriaDTO objeto)
        {
            Respuesta<bool> rpt = new Respuesta<bool>()
            {
                Estado = false,
                Data = false,
                Mensaje = "Error desconocido"
            };

            int resultado = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GuardarConvocatoria", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdMiembro", objeto.IdMiembro);
                        cmd.Parameters.AddWithValue("@IdsJugadores", objeto.IdsJugadores ?? "");

                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        resultado = Convert.ToInt32(outputParam.Value);
                    }
                }

                if (resultado == 1)
                {
                    rpt.Estado = true;
                    rpt.Data = true;
                    rpt.Mensaje = "Convocatoria guardada correctamente.";
                }
                else if (resultado == -1)
                {
                    rpt.Mensaje = "El técnico no tiene un equipo asignado o no existe.";
                }
                else
                {
                    rpt.Mensaje = "Ocurrió un error interno al actualizar la plantilla.";
                }
            }
            catch (Exception ex)
            {
                rpt.Estado = false;
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }

    }
}