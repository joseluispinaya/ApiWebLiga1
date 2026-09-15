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
    public class UsuarioData
    {
        public static Respuesta<UsuarioDTO> LoginUsuarioTecnico(string NroCi)
        {
            try
            {
                UsuarioDTO obj = null;

                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_LoginAppTecnico", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CI", NroCi);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new UsuarioDTO
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdMiembro"]),
                                    IdAcceso = Convert.ToInt32(dr["IdAcceso"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    CI = dr["CI"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    ClaveHash = dr["ClaveHash"].ToString()
                                };
                            }
                        }
                    }
                }

                return new Respuesta<UsuarioDTO>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Bienvenido" : "Usuario o Contraseña incorrectos."
                };
            }
            catch (Exception)
            {
                return new Respuesta<UsuarioDTO>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error en el servidor. Intente más tarde.",
                    Data = null
                };
            }
        }

        public static Respuesta<UsuarioDTO> LoginUsuarioJugador(string NroCi)
        {
            try
            {
                UsuarioDTO obj = null;

                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_LoginAppJugador", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CI", NroCi);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new UsuarioDTO
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdJugador"]),
                                    IdAcceso = Convert.ToInt32(dr["IdAcceso"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    CI = dr["CI"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    ClaveHash = dr["ClaveHash"].ToString()
                                };
                            }
                        }
                    }
                }

                return new Respuesta<UsuarioDTO>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Bienvenido" : "Usuario o Contraseña incorrectos."
                };
            }
            catch (Exception)
            {
                return new Respuesta<UsuarioDTO>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error en el servidor. Intente más tarde.",
                    Data = null
                };
            }
        }

        public static void ActualizarTokenTecni(int IdMiembro, string ExpoPushToken)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ActualizarTokenTecni", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdMiembro", IdMiembro);
                        comando.Parameters.AddWithValue("@ExpoPushToken", ExpoPushToken);

                        con.Open();
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Se captura el error pero no se lanza (throw) para no detener la ejecucion
            }
        }

        public static void ActualizarTokenJuga(int IdJugador, string ExpoPushToken)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ActualizarTokenJuga", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdJugador", IdJugador);
                        comando.Parameters.AddWithValue("@ExpoPushToken", ExpoPushToken);

                        con.Open();
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Se captura el error pero no se lanza (throw) para no detener la ejecucion
            }
        }

        public static Respuesta<UsuarioDTO> LoginUsuarioPublico(string NroCi)
        {
            try
            {
                UsuarioDTO obj = null;

                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_LoginAppPublico", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@CI", NroCi);

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new UsuarioDTO
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdPublico"]),
                                    IdAcceso = Convert.ToInt32(dr["IdAcceso"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    CI = dr["CI"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    ClaveHash = dr["ClaveHash"].ToString()
                                };
                            }
                        }
                    }
                }

                return new Respuesta<UsuarioDTO>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Bienvenido" : "Usuario o Contraseña incorrectos."
                };
            }
            catch (Exception)
            {
                return new Respuesta<UsuarioDTO>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error en el servidor. Intente más tarde.",
                    Data = null
                };
            }
        }

        public static void ActualizarTokenPublico(int IdPublico, string ExpoPushToken)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_ActualizarTokenPublico", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdPublico", IdPublico);
                        comando.Parameters.AddWithValue("@ExpoPushToken", ExpoPushToken);

                        con.Open();
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Se captura el error pero no se lanza (throw) para no detener la ejecucion
            }
        }

        public static Respuesta<int> RegistrarPublicoApp(PublicoDTO objeto)
        {
            Respuesta<int> response = new Respuesta<int>();
            int resultadoCodigo = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_RegistrarPublicoApp", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Nombres", objeto.Nombres);
                        cmd.Parameters.AddWithValue("@Apellidos", objeto.Apellidos);
                        cmd.Parameters.AddWithValue("@CI", objeto.CI);
                        cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                        cmd.Parameters.AddWithValue("@ClaveHash", objeto.ClaveHash);

                        // Manejo seguro de NULL para el token
                        cmd.Parameters.AddWithValue("@ExpoPushToken", string.IsNullOrEmpty(objeto.ExpoPushToken) ? (object)DBNull.Value : objeto.ExpoPushToken);

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
                        response.Mensaje = "Ya existe un nro de CI o Correo en el sistema.";
                        break;

                    case 2: // Registrado
                        response.Estado = true;
                        response.Valor = "success";
                        response.Mensaje = "Registrado correctamente.";
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

        public static Respuesta<bool> ActualizarCorreo(ActualizarCorreoDTO objeto)
        {
            Respuesta<bool> response = new Respuesta<bool>()
            {
                Estado = false,
                Data = false,
                Mensaje = "Error desconocido"
            };

            int resultadoCodigo = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_ActualizarCorreoApp", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@IdUsuario", objeto.IdUsuario);
                        cmd.Parameters.AddWithValue("@IdAcceso", objeto.IdAcceso);

                        // Aseguramos que se guarde en minúsculas por seguridad adicional
                        cmd.Parameters.AddWithValue("@Correo", objeto.Correo?.Trim().ToLower());

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

                if (resultadoCodigo == 1)
                {
                    response.Estado = true;
                    response.Data = true;
                    response.Mensaje = "Correo actualizado correctamente.";
                }
                else if (resultadoCodigo == -1)
                {
                    response.Mensaje = "El tipo de acceso no es válido.";
                }
                else
                {
                    response.Mensaje = "Ocurrió un error al actualizar el correo en la base de datos.";
                }
            }
            catch (Exception ex)
            {
                response.Estado = false;
                response.Mensaje = "Error interno: " + ex.Message;
            }

            return response;
        }

        public static Respuesta<UsuarioDTO> BuscarUsuarioCorreo(RecuperarDTO objeto)
        {
            try
            {
                UsuarioDTO obj = null;

                using (SqlConnection con = new SqlConnection(Conexion.RutaConexion))
                {
                    using (SqlCommand comando = new SqlCommand("usp_BuscarUsuarioPorCorreo", con))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdAcceso", objeto.IdAcceso);
                        comando.Parameters.AddWithValue("@Correo", objeto.Correo?.Trim().ToLower());

                        con.Open();
                        using (SqlDataReader dr = comando.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                obj = new UsuarioDTO
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    IdAcceso = Convert.ToInt32(dr["IdAcceso"]),
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    CI = dr["CI"].ToString(),
                                    Correo = dr["Correo"].ToString()
                                };
                            }
                        }
                    }
                }

                return new Respuesta<UsuarioDTO>
                {
                    Estado = obj != null,
                    Data = obj,
                    Mensaje = obj != null ? "Exito" : "No se encontró el usuario con el correo proporcionado."
                };
            }
            catch (Exception)
            {
                return new Respuesta<UsuarioDTO>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error en el servidor. Intente más tarde.",
                    Data = null
                };
            }
        }

    }
}