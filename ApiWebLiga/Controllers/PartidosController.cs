using ApiWebLiga.Data;
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
    [RoutePrefix("api/partidos")]
    public class PartidosController : ApiController
    {
        [HttpGet]
        [Route("listaPartidos")]
        public IHttpActionResult ListaPartidos()
        {
            var respuesta = TorneoData.ListaPartidos();

            return Ok(respuesta);
        }

        [HttpGet]
        [Route("arbitraje/{idMiembro:int}/deudas")]
        public IHttpActionResult DeudasArbitrajeTecnico(int idMiembro)
        {
            var respuesta = TorneoData.DeudasArbitrajePorTecnico(idMiembro);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("sanciones/{idMiembro:int}/partido")]
        public IHttpActionResult DeudaSancionesTecnico(int idMiembro)
        {
            var respuesta = TorneoData.DeudasSancionesPorTecnico(idMiembro);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("detalle/{idPartido:int}/{idEquipo:int}/sanciones")]
        public IHttpActionResult DetalleSanciones(int idPartido, int idEquipo)
        {
            var respuesta = TorneoData.ObtenerDetalleSanciones(idPartido, idEquipo);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("planilla/{idMiembro:int}/equipo")]
        public IHttpActionResult ObtenerPlantillaEquipo(int idMiembro)
        {
            var respuesta = TorneoData.ObtenerPlantillaEquipo(idMiembro);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("enfrentamiento/{id:int}/detalle")]
        public IHttpActionResult ObtenerDetalleEnfrentamiento(int id)
        {
            var respuesta = TorneoData.ObtenerDetallePartidoNotificacion(id);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("evento/{id:int}/detalleVivo")]
        public IHttpActionResult DetalleEnVivo(int id)
        {
            var respuesta = TorneoData.ObtenerDetalleEnVivoApp(id);
            return Ok(respuesta);
        }

    }
}