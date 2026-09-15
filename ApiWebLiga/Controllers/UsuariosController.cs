using ApiWebLiga.Data;
using ApiWebLiga.Helpers;
using ApiWebLiga.Models;
using ApiWebLiga.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiWebLiga.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/usuarios")]
    public class UsuariosController : ApiController
    {
        [HttpPost]
        [Route("registroPublico")]
        public IHttpActionResult RegistrarPublico([FromBody] PublicoDTO request)
        {
            if (request == null)
            {
                return Ok(new Respuesta<int>
                {
                    Estado = false,
                    Valor = "warning",
                    Mensaje = "Debe enviar los datos requeridos."
                });
            }

            try
            {
                // Hasheamos la clave
                request.ClaveHash = Utilidadesj.Hash(request.ClaveHash);

                var respuesta = UsuarioData.RegistrarPublicoApp(request);

                return Ok(respuesta);
            }
            catch (Exception)
            {
                return Ok(new Respuesta<int>
                {
                    Estado = false,
                    Valor = "error",
                    Mensaje = "Ocurrió un error interno en el servidor"
                });
            }
        }

        [HttpPost]
        [Route("actualizarCorreo")]
        public IHttpActionResult ActualizarCorreoApp([FromBody] ActualizarCorreoDTO peticion)
        {
            try
            {
                if (peticion == null || string.IsNullOrWhiteSpace(peticion.Correo) || peticion.IdUsuario <= 0)
                {
                    return Ok(new Respuesta<bool>
                    {
                        Estado = false,
                        Data = false,
                        Mensaje = "Los datos enviados están incompletos o son incorrectos."
                    });
                }

                var respuesta = UsuarioData.ActualizarCorreo(peticion); // Ajusta UsuarioData si tu clase se llama distinto

                return Ok(respuesta);
            }
            catch (Exception)
            {
                return Ok(new Respuesta<bool>
                {
                    Estado = false,
                    Data = false,
                    Mensaje = "Ocurrió un error inesperado al procesar la solicitud."
                });
            }
        }

        [HttpPost]
        [Route("restablecerAcceso")]
        public IHttpActionResult SolicitudRecuperacion([FromBody] RecuperarDTO peticion)
        {
            try
            {
                if (peticion == null || string.IsNullOrWhiteSpace(peticion.Correo) || peticion.IdAcceso <= 0)
                {
                    return Ok(new Respuesta<bool>
                    {
                        Estado = false,
                        Mensaje = "Los datos enviados están incompletos o son incorrectos."
                    });
                }

                var respuesta = UsuarioData.BuscarUsuarioCorreo(peticion); // Ajusta UsuarioData si tu clase se llama distinto

                if (!respuesta.Estado || respuesta.Data == null)
                {
                    return Ok(new Respuesta<bool>
                    {
                        Estado = false,
                        Mensaje = respuesta.Mensaje
                    });
                }

                var objUser = respuesta.Data;

                var tokenSesion = Guid.NewGuid().ToString("N");
                // agregar el IdAcceso a la url
                string confirmUrl = $"https://joseluis1989-012-site2.ftempurl.com/RestablecerAcceso.aspx?email={objUser.Correo}&token={tokenSesion}&idacceso={objUser.IdAcceso}";
                bool correoEnviado = Utilidadesj.EnviosCorreos(objUser.Correo, "Recuperar Acceso", objUser.Nombres, confirmUrl);

                return Ok(new Respuesta<bool>
                {
                    Estado = correoEnviado,
                    Mensaje = correoEnviado ? "Recuperacion exitosa. Verifique su correo." : "Error al recuperar. No se pudo enviar acceso a su correo."
                });
            }
            catch (Exception)
            {
                return Ok(new Respuesta<bool>
                {
                    Estado = false,
                    Mensaje = "Ocurrió un error inesperado al procesar la solicitud."
                });
            }
        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult InicioSession(LoginDTO loginDTO)
        {
            try
            {
                // Validación de entrada
                if (loginDTO == null || string.IsNullOrWhiteSpace(loginDTO.NroCi) || string.IsNullOrWhiteSpace(loginDTO.Clave))
                {
                    return Ok(RespuestaError("Debe ingresar su nro CI y una contraseña para iniciar sesión."));
                }

                if (loginDTO.IdAcceso != 1 && loginDTO.IdAcceso != 2 && loginDTO.IdAcceso != 3)
                {
                    return Ok(RespuestaError("Debe seleccionar un tipo de acceso válido."));
                }

                // Variable para guardar la respuesta de cualquiera de los tres métodos
                Respuesta<UsuarioDTO> respuestaLogin;

                // Evaluamos el tipo de acceso que envió la app
                if (loginDTO.IdAcceso == 1)
                {
                    respuestaLogin = LoginUsuarioJugador(loginDTO.NroCi, loginDTO.Clave, loginDTO.ExpoPushToken);
                }
                else if (loginDTO.IdAcceso == 2)
                {
                    respuestaLogin = LoginUsuarioTecnico(loginDTO.NroCi, loginDTO.Clave, loginDTO.ExpoPushToken);
                }
                else
                {
                    respuestaLogin = LoginUsuarioPublico(loginDTO.NroCi, loginDTO.Clave, loginDTO.ExpoPushToken);
                }

                // Retornamos directamente el resultado (que ya viene formateado con éxito o error desde tus métodos privados)
                return Ok(respuestaLogin);
            }
            catch (Exception)
            {
                return Ok(RespuestaError("Ocurrió un error inesperado al iniciar sesión"));
            }
        }

        private Respuesta<UsuarioDTO> LoginUsuarioTecnico(string nroCi, string clave, string expoPushToken)
        {
            try
            {
                var resp = UsuarioData.LoginUsuarioTecnico(nroCi);

                if (!resp.Estado || resp.Data == null)
                {
                    return RespuestaError(resp.Mensaje ?? "Credenciales incorrectas.");
                }

                var objUser = resp.Data;

                // Verificamos la contraseña (BCrypt)
                bool passCorrecta = Utilidadesj.Verify(clave, objUser.ClaveHash);

                if (!passCorrecta)
                {
                    return RespuestaError("Credenciales incorrectas.");
                }

                if (!string.IsNullOrWhiteSpace(expoPushToken))
                {
                    // Llamamos a tu método ActualizarToken
                    UsuarioData.ActualizarTokenTecni(objUser.IdUsuario, expoPushToken);
                }

                objUser.ClaveHash = "";

                return new Respuesta<UsuarioDTO>
                {
                    Estado = true,
                    Data = objUser,
                    Mensaje = "Sistema deportivo"
                };
            }
            catch (Exception)
            {
                return RespuestaError("Ocurrió un error inesperado al iniciar sesión");
            }
        }

        private Respuesta<UsuarioDTO> LoginUsuarioJugador(string nroCi, string clave, string expoPushToken)
        {
            try
            {
                var resp = UsuarioData.LoginUsuarioJugador(nroCi);

                if (!resp.Estado || resp.Data == null)
                {
                    return RespuestaError(resp.Mensaje ?? "Credenciales incorrectas.");
                }

                var objUser = resp.Data;

                // Verificamos la contraseña (BCrypt)
                bool passCorrecta = Utilidadesj.Verify(clave, objUser.ClaveHash);

                if (!passCorrecta)
                {
                    return RespuestaError("Credenciales incorrectas.");
                }

                if (!string.IsNullOrWhiteSpace(expoPushToken))
                {
                    // Llamamos a tu método ActualizarToken
                    UsuarioData.ActualizarTokenJuga(objUser.IdUsuario, expoPushToken);
                }

                objUser.ClaveHash = "";

                return new Respuesta<UsuarioDTO>
                {
                    Estado = true,
                    Data = objUser,
                    Mensaje = "Sistema deportivo"
                };
            }
            catch (Exception)
            {
                return RespuestaError("Ocurrió un error inesperado al iniciar sesión");
            }
        }

        private Respuesta<UsuarioDTO> LoginUsuarioPublico(string nroCi, string clave, string expoPushToken)
        {
            try
            {
                var resp = UsuarioData.LoginUsuarioPublico(nroCi);

                if (!resp.Estado || resp.Data == null)
                {
                    return RespuestaError(resp.Mensaje ?? "Credenciales incorrectas.");
                }

                var objUser = resp.Data;

                // Verificamos la contraseña (BCrypt)
                bool passCorrecta = Utilidadesj.Verify(clave, objUser.ClaveHash);

                if (!passCorrecta)
                {
                    return RespuestaError("Credenciales incorrectas.");
                }

                if (!string.IsNullOrWhiteSpace(expoPushToken))
                {
                    // Llamamos a tu método ActualizarToken
                    UsuarioData.ActualizarTokenPublico(objUser.IdUsuario, expoPushToken);
                }

                objUser.ClaveHash = "";

                return new Respuesta<UsuarioDTO>
                {
                    Estado = true,
                    Data = objUser,
                    Mensaje = "Sistema deportivo"
                };
            }
            catch (Exception)
            {
                return RespuestaError("Ocurrió un error inesperado al iniciar sesión");
            }
        }

        private Respuesta<UsuarioDTO> RespuestaError(string mensaje)
        {
            return new Respuesta<UsuarioDTO>
            {
                Estado = false,
                Mensaje = mensaje,
                Data = null
            };
        }
    }
}