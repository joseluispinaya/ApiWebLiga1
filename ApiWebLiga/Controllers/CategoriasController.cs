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
    // Definimos la ruta base para este controlador
    [RoutePrefix("api/categorias")]
    public class CategoriasController : ApiController
    {
        [HttpGet]
        [Route("lista")]
        public IHttpActionResult ObtenerCategorias()
        {
            var respuesta = CategoriaData.ListaCategorias();

            // Siempre devolvemos HTTP 200 (Ok), el Frontend leerá el campo "Estado"
            return Ok(respuesta);
        }

    }
}