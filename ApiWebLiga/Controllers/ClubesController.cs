using ApiWebLiga.Data;
using ApiWebLiga.Helpers;
using ApiWebLiga.Models;
using ApiWebLiga.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ApiWebLiga.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    // Definimos la ruta base para este controlador
    [RoutePrefix("api/clubes")]
    public class ClubesController : ApiController
    {
        [HttpGet]
        [Route("listaClubes")]
        public IHttpActionResult ListaClubes()
        {
            var respuesta = ClubData.ListaClubes();

            return Ok(respuesta);
        }

        [HttpPost]
        [Route("registroClub")]
        public IHttpActionResult GuardarOrEditClub([FromBody] Club request)
        {
            // 1. Validación inicial del objeto
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
                string logoUrl = string.Empty;

                // 1. Validar y convertir Fecha de forma segura
                if (!DateTime.TryParseExact(request.FechaFundacion, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaFundacion))
                {
                    return Ok(new Respuesta<int>
                    {
                        Estado = false,
                        Valor = "warning",
                        Mensaje = "El formato de la fecha no es válido. Debe ser dd/MM/yyyy."
                    });

                }

                // 3. Manejo del logo
                if (!string.IsNullOrEmpty(request.Base64Image))
                {
                    byte[] imageBytes = Convert.FromBase64String(request.Base64Image);
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        string folder = "/Logos/";
                        logoUrl = Utilidadesj.UploadPhoto(stream, folder);
                    }
                }

                request.LogoUrl = logoUrl;

                var respuesta = ClubData.GuardarOrEditClub(request, fechaFundacion);
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

    }
}