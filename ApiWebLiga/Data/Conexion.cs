using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ApiWebLiga.Data
{
    public class Conexion
    {
        public static string RutaConexion = ConfigurationManager.ConnectionStrings["ConexionCadenas"].ConnectionString;
    }
}