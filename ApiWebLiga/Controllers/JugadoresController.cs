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
    [RoutePrefix("api/jugadores")]
    public class JugadoresController : ApiController
    {
        [HttpGet]
        [Route("jugador/{idJugador:int}/fichaTecnica")]
        public IHttpActionResult FichaTecnicaJugador(int idJugador)
        {
            var respuesta = JugadorData.FichaTecnicaJugador(idJugador);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("tecnico/{idMiembro:int}/dashboard")]
        public IHttpActionResult DashboardTecnico(int idMiembro)
        {
            var respuesta = JugadorData.DashboardTecnico(idMiembro);
            return Ok(respuesta);
        }
    }
}