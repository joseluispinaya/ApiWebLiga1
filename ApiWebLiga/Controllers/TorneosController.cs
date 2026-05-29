using ApiWebLiga.Data;
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
    // Definimos la ruta base para este controlador
    [RoutePrefix("api/torneos")]
    public class TorneosController : ApiController
    {
        [HttpGet]
        [Route("listaTorneos")]
        public IHttpActionResult ListaTorneos()
        {
            var respuesta = TorneoData.ListaTorneos();

            // Siempre devolvemos HTTP 200 (Ok), el Frontend leerá el campo "Estado"
            return Ok(respuesta);
        }

        [HttpPost]
        [Route("registrarTorneo")]
        public IHttpActionResult GuardarOrEditTorneos([FromBody] Torneo objeto)
        {
            if (objeto == null)
            {
                return Ok(new Respuesta<int>
                {
                    Estado = false,
                    Mensaje = "Datos no recibidos",
                    Valor = "warning"
                });
            }

            var respuesta = TorneoData.GuardarOrEditTorneos(objeto);
            return Ok(respuesta);

        }
    }
}