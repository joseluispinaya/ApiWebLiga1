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
    public class JugadorData
    {
        public static Respuesta<JugadorDTO> FichaTecnicaJugador(int IdJugador)
        {
            try
            {
                JugadorDTO obj = null;

                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_FichaTecnicaJugador", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdJugador", IdJugador);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new JugadorDTO
                                {
                                    NombreJugador = dr["NombreJugador"].ToString(),
                                    NroComet = dr["NroComet"].ToString(),
                                    NroCi = dr["NroCi"].ToString(),
                                    FechaNacimientoStr = dr["FechaNacimientoStr"].ToString(),
                                    Edad = Convert.ToInt32(dr["Edad"]),
                                    FotografiaUrl = dr["FotografiaUrl"].ToString(),
                                    NombreClub = dr["NombreClub"].ToString(),
                                    LogoUrl = dr["LogoUrl"].ToString(),
                                    NroCamiseta = Convert.ToInt32(dr["NroCamiseta"]),
                                };
                            }
                        }
                    }
                }

                return new Respuesta<JugadorDTO>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Informacion Encontrada" : "No se pudo encontrar Informacion."
                };
            }
            catch (Exception)
            {
                return new Respuesta<JugadorDTO>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error en el servidor. Intente más tarde.",
                    Data = null
                };
            }
        }

        public static Respuesta<DashboardTecnicoDTO> DashboardTecnico(int IdMiembro)
        {
            try
            {
                DashboardTecnicoDTO obj = null;

                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_DashboardTecnico", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdMiembro", IdMiembro);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new DashboardTecnicoDTO
                                {
                                    NombreTecnico = dr["NombreTecnico"].ToString(),
                                    CargoTecnico = dr["CargoTecnico"].ToString(),

                                    NombreClub = dr["NombreClub"].ToString(),
                                    LogoClub = dr["LogoClub"] != DBNull.Value ? dr["LogoClub"].ToString() : "", // O puedes poner tu ruta "Logos/sinLogo.png"
                                    NombreCategoria = dr["NombreCategoria"].ToString(),
                                    NombreTorneo = dr["NombreTorneo"].ToString(),
                                    NombreSerie = dr["NombreSerie"].ToString(),

                                    // Parseamos el único entero
                                    TotalJugadores = Convert.ToInt32(dr["TotalJugadores"]),

                                    FechaProximoPartido = dr["FechaProximoPartido"].ToString(),
                                    HoraProximoPartido = dr["HoraProximoPartido"].ToString(),
                                    CanchaProximoPartido = dr["CanchaProximoPartido"].ToString(),
                                    NombreRival = dr["NombreRival"].ToString()
                                };
                            }
                        }
                    }
                }

                return new Respuesta<DashboardTecnicoDTO>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Información del Dashboard cargada correctamente." : "No se encontró información del equipo para este técnico."
                };
            }
            catch (Exception)
            {
                // Opcional: Aquí podrías registrar el "ex.Message" en un log si lo necesitas
                return new Respuesta<DashboardTecnicoDTO>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error en el servidor. Intente más tarde.",
                    Data = null
                };
            }
        }

    }
}