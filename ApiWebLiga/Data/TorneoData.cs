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
    public class TorneoData
    {
        public static Respuesta<List<Torneo>> ListaTorneos()
        {
            // 1. Iniciamos la respuesta por defecto en "Error" por si algo falla
            Respuesta<List<Torneo>> rpt = new Respuesta<List<Torneo>>()
            {
                Estado = false,
                Data = new List<Torneo>(), // Lista vacía, no nula
                Mensaje = "Error desconocido"
            };

            try
            {
                // Usamos la cadena limpia del Web.config
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_TorneosListar", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new Torneo
                                {
                                    IdTorneo = Convert.ToInt32(dr["IdTorneo"]),
                                    NombreTorneo = dr["NombreTorneo"].ToString(),
                                    Gestion = Convert.ToInt32(dr["Gestion"]),
                                    PuntosVictoriaLocal = Convert.ToInt32(dr["PuntosVictoriaLocal"]),
                                    PuntosVictoriaVisitante = Convert.ToInt32(dr["PuntosVictoriaVisitante"]),
                                    PuntosEmpateLocal = Convert.ToInt32(dr["PuntosEmpateLocal"]),
                                    PuntosEmpateVisitante = Convert.ToInt32(dr["PuntosEmpateVisitante"]),
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

        public static Respuesta<List<PartidosDTO>> ListaPartidos()
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
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerPartidosProgramados", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
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

        public static Respuesta<List<DeudasArbitrajeDTO>> DeudasArbitrajePorTecnico(int IdMiembro)
        {
            Respuesta<List<DeudasArbitrajeDTO>> rpt = new Respuesta<List<DeudasArbitrajeDTO>>()
            {
                Estado = false,
                Data = new List<DeudasArbitrajeDTO>(),
                Mensaje = "Error desconocido"
            };

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_DeudasArbitrajePorTecnico", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdMiembro", IdMiembro);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new DeudasArbitrajeDTO
                                {
                                    IdPartido = Convert.ToInt32(dr["IdPartido"]),
                                    Fecha = dr["Fecha"].ToString(),
                                    Hora = dr["Hora"].ToString(),
                                    Cancha = dr["Cancha"].ToString(),
                                    NombreFase = dr["NombreFase"].ToString(),

                                    IdEquipo = Convert.ToInt32(dr["IdEquipo"]),
                                    NombreClub = dr["NombreClub"].ToString(),
                                    LogoClub = dr["LogoClub"] != DBNull.Value ? dr["LogoClub"].ToString() : "",
                                    Condicion = dr["Condicion"].ToString(),

                                    NombreRival = dr["NombreRival"].ToString(),
                                    LogoRival = dr["LogoRival"] != DBNull.Value ? dr["LogoRival"].ToString() : "",
                                    MarcadorFinal = dr["MarcadorFinal"].ToString()
                                });
                            }
                        }
                    }
                }

                rpt.Estado = true;
                rpt.Mensaje = "Deudas de arbitraje obtenidas correctamente.";
            }
            catch (Exception ex)
            {
                rpt.Estado = false;
                rpt.Data = new List<DeudasArbitrajeDTO>();
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }

        public static Respuesta<List<DeudasSancionesTecnicoDTO>> DeudasSancionesPorTecnico(int idMiembro)
        {
            Respuesta<List<DeudasSancionesTecnicoDTO>> rpt = new Respuesta<List<DeudasSancionesTecnicoDTO>>()
            {
                Estado = false,
                Data = new List<DeudasSancionesTecnicoDTO>(),
                Mensaje = "Error desconocido"
            };

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_DeudasSancionesPorTecnico", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdMiembro", idMiembro);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new DeudasSancionesTecnicoDTO
                                {
                                    IdPartido = Convert.ToInt32(dr["IdPartido"]),
                                    Fecha = dr["Fecha"].ToString(),
                                    Hora = dr["Hora"].ToString(),
                                    Cancha = dr["Cancha"].ToString(),
                                    NombreFase = dr["NombreFase"].ToString(),
                                    IdEquipo = Convert.ToInt32(dr["IdEquipo"]),
                                    NombreRival = dr["NombreRival"].ToString(),
                                    NroSanciones = Convert.ToInt32(dr["NroSanciones"]),
                                    DeudaTotal = Convert.ToDecimal(dr["DeudaTotal"])
                                });
                            }
                        }
                    }
                }

                rpt.Estado = true;
                rpt.Mensaje = "Deudas por sanciones obtenidas correctamente.";
            }
            catch (Exception ex)
            {
                rpt.Estado = false;
                rpt.Data = new List<DeudasSancionesTecnicoDTO>();
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }

        public static Respuesta<List<DetalleSancionDTO>> ObtenerDetalleSanciones(int IdPartido, int IdEquipo)
        {
            Respuesta<List<DetalleSancionDTO>> rpt = new Respuesta<List<DetalleSancionDTO>>()
            {
                Estado = false,
                Data = new List<DetalleSancionDTO>(),
                Mensaje = "Error desconocido"
            };

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_DetalleSancionesEquipoPartido", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdPartido", IdPartido);
                        comando.Parameters.AddWithValue("@IdEquipo", IdEquipo);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new DetalleSancionDTO
                                {
                                    IdSancion = Convert.ToInt32(dr["IdSancion"]),
                                    Minuto = Convert.ToInt32(dr["Minuto"]),
                                    TipoEvento = dr["TipoEvento"].ToString(),
                                    IdTipoEvento = Convert.ToInt32(dr["IdTipoEvento"]),
                                    NombreJugador = dr["NombreJugador"].ToString(),
                                    Dorsal = Convert.ToInt32(dr["Dorsal"]),
                                    Monto = Convert.ToDecimal(dr["Monto"])
                                });
                            }
                        }
                    }
                }

                rpt.Estado = true;
                rpt.Mensaje = "Detalles obtenidas correctamente.";
            }
            catch (Exception ex)
            {
                rpt.Estado = false;
                rpt.Data = new List<DetalleSancionDTO>();
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }

        public static Respuesta<List<PlantillaJugadorDTO>> ObtenerPlantillaEquipo(int IdMiembro)
        {
            Respuesta<List<PlantillaJugadorDTO>> rpt = new Respuesta<List<PlantillaJugadorDTO>>()
            {
                Estado = false,
                Data = new List<PlantillaJugadorDTO>(),
                Mensaje = "Error desconocido"
            };

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_PlantillaPorTecnico", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdMiembro", IdMiembro);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                rpt.Data.Add(new PlantillaJugadorDTO
                                {
                                    IdJugador = Convert.ToInt32(dr["IdJugador"]),
                                    Dorsal = Convert.ToInt32(dr["Dorsal"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    CI = dr["CI"].ToString(),
                                    FotografiaUrl = dr["FotografiaUrl"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                });
                            }
                        }
                    }
                }

                rpt.Estado = true;
                rpt.Mensaje = "Planilla obtenidas correctamente.";
            }
            catch (Exception ex)
            {
                rpt.Estado = false;
                rpt.Data = new List<PlantillaJugadorDTO>();
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }

        public static Respuesta<DetallePartidoNotificacionDTO> ObtenerDetallePartidoNotificacion(int idPartido)
        {
            Respuesta<DetallePartidoNotificacionDTO> rpt = new Respuesta<DetallePartidoNotificacionDTO>()
            {
                Estado = false,
                Data = null,
                Mensaje = "No se encontró la información del partido."
            };

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerDetallePartidoApp", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdPartido", idPartido);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                rpt.Data = new DetallePartidoNotificacionDTO
                                {
                                    Fecha = dr["Fecha"].ToString(),
                                    Hora = dr["Hora"].ToString(),
                                    Cancha = dr["Cancha"].ToString(),
                                    NombreFase = dr["NombreFase"].ToString(),
                                    ClubLocal = dr["ClubLocal"].ToString(),
                                    LogoLocal = dr["LogoLocal"] != DBNull.Value ? dr["LogoLocal"].ToString() : "",
                                    ClubVisitante = dr["ClubVisitante"].ToString(),
                                    LogoVisitante = dr["LogoVisitante"] != DBNull.Value ? dr["LogoVisitante"].ToString() : ""
                                };
                                rpt.Estado = true;
                                rpt.Mensaje = "Detalle del partido obtenido correctamente.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                rpt.Estado = false;
                rpt.Data = null;
                rpt.Mensaje = $"Error en BD: {ex.Message}";
            }

            return rpt;
        }

        public static Respuesta<PartidoEnVivoAppDTO> ObtenerDetalleEnVivoApp(int idPartido)
        {
            Respuesta<PartidoEnVivoAppDTO> rpt = new Respuesta<PartidoEnVivoAppDTO>()
            {
                Estado = false,
                Data = new PartidoEnVivoAppDTO(),
                Mensaje = "No se pudo obtener la información."
            };

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ObtenerDetalleEnVivoApp", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdPartido", idPartido);
                        con.Open();

                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            // 1. LECTURA DE LA CABECERA
                            if (dr.Read())
                            {
                                rpt.Data.Cabecera = new CabeceraPartidoAppDTO
                                {
                                    Fecha = dr["Fecha"].ToString(),
                                    Hora = dr["Hora"].ToString(),
                                    NombreFase = dr["NombreFase"].ToString(),
                                    NombreEstado = dr["NombreEstado"].ToString(),

                                    IdEquipoLocal = Convert.ToInt32(dr["IdEquipoLocal"]),
                                    ClubLocal = dr["ClubLocal"].ToString(),
                                    LogoLocal = dr["LogoLocal"] != DBNull.Value ? dr["LogoLocal"].ToString() : "",
                                    GolesLocal = Convert.ToInt32(dr["GolesLocal"]),

                                    IdEquipoVisitante = Convert.ToInt32(dr["IdEquipoVisitante"]),
                                    ClubVisitante = dr["ClubVisitante"].ToString(),
                                    LogoVisitante = dr["LogoVisitante"] != DBNull.Value ? dr["LogoVisitante"].ToString() : "",
                                    GolesVisitante = Convert.ToInt32(dr["GolesVisitante"])
                                };
                            }

                            // 2. PASAMOS AL SEGUNDO SELECT (LOS EVENTOS)
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    rpt.Data.Eventos.Add(new EventoAppDTO
                                    {
                                        Minuto = Convert.ToInt32(dr["Minuto"]),
                                        IdTipoEvento = Convert.ToInt32(dr["IdTipoEvento"]),
                                        NombreJugador = dr["NombreJugador"].ToString(),
                                        IdEquipo = Convert.ToInt32(dr["IdEquipo"])
                                    });
                                }
                            }
                        }
                    }
                }

                if (rpt.Data.Cabecera != null)
                {
                    rpt.Estado = true;
                    rpt.Mensaje = "Datos obtenidos correctamente.";
                }
            }
            catch (Exception ex)
            {
                rpt.Estado = false;
                rpt.Data = null;
                rpt.Mensaje = $"Error BD: {ex.Message}";
            }

            return rpt;
        }

    }
}