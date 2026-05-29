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
    public class ClubData
    {
        public static Respuesta<List<Club>> ListaClubes()
        {
            // 1. Iniciamos la respuesta por defecto en "Error" por si algo falla
            Respuesta<List<Club>> rpt = new Respuesta<List<Club>>()
            {
                Estado = false,
                Data = new List<Club>(), // Lista vacía, no nula
                Mensaje = "Error desconocido"
            };

            try
            {
                // Usamos la cadena limpia del Web.config
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ClubesListar", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new Club
                                {
                                    IdClub = Convert.ToInt32(dr["IdClub"]),
                                    NombreClub = dr["NombreClub"].ToString(),
                                    LogoUrl = dr["LogoUrl"].ToString(),
                                    FechaFundacion = Convert.ToDateTime(dr["FechaFundacion"]).ToString("dd/MM/yyyy"),
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

        public static Respuesta<int> GuardarOrEditClub(Club objeto, DateTime FechaFundacion)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GuardarOrEditClubes", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdClub", objeto.IdClub);
                        cmd.Parameters.AddWithValue("@NombreClub", objeto.NombreClub);
                        cmd.Parameters.AddWithValue("@LogoUrl", string.IsNullOrEmpty(objeto.LogoUrl) ? "" : objeto.LogoUrl);

                        cmd.Parameters.AddWithValue("@FechaFundacion", FechaFundacion);
                        cmd.Parameters.AddWithValue("@Estado", objeto.Estado);

                        SqlParameter outputParam = new SqlParameter("@Resultado", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputParam);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        resultadoCodigo = Convert.ToInt32(outputParam.Value);
                    }
                }

                response.Data = resultadoCodigo;

                switch (resultadoCodigo)
                {
                    case 1: // duplicado validar
                        response.Estado = false;
                        response.Valor = "warning";
                        response.Mensaje = "Ya existe un club con ese nombre.";
                        break;

                    case 2: // Registrado
                        response.Estado = true;
                        response.Valor = "success";
                        response.Mensaje = "Registrado correctamente.";
                        break;

                    case 3: // Actualizado
                        response.Estado = true;
                        response.Valor = "success";
                        response.Mensaje = "Actualizado correctamente.";
                        break;

                    case 0: // Error
                    default:
                        response.Estado = false;
                        response.Valor = "error";
                        response.Mensaje = "No se pudo completar la operación.";
                        break;
                }
            }
            catch (Exception ex)
            {
                //response.Data = 0;
                response.Estado = false;
                response.Valor = "error";
                response.Mensaje = "Error interno: " + ex.Message;
            }

            return response;
        }

    }
}