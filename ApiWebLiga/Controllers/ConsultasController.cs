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
    [RoutePrefix("api/consultas")]
    public class ConsultasController : ApiController
    {
        [HttpGet]
        [Route("listaCategorias")]
        public IHttpActionResult ListaCategorias()
        {
            var respuesta = PartidosCatData.ListaCategoriasConPartidos();

            return Ok(respuesta);
        }

        [HttpGet]
        [Route("categoria/{idCategoria:int}/partidos")]
        public IHttpActionResult ListaPartidosCategoria(int idCategoria)
        {
            var respuesta = PartidosCatData.ListaPartidosCategoria(idCategoria);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("clubesLista")]
        public IHttpActionResult ListaClubesApp()
        {
            var respuesta = ClubData.ListaClubesApp();

            return Ok(respuesta);
        }

        [HttpPost]
        [Route("guardarConvocatoria")]
        public IHttpActionResult GuardarConvocatoria([FromBody] ConvocatoriaDTO objeto)
        {
            // Validación básica
            if (objeto == null)
            {
                return Ok(new Respuesta<bool> { Estado = false, Mensaje = "Datos vacíos o mal formados." });
            }

            var respuesta = PartidosCatData.GuardarConvocatoria(objeto);
            return Ok(respuesta);
        }

    }
}