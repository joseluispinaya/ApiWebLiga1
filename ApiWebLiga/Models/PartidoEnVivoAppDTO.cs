using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiWebLiga.Models
{
    public class PartidoEnVivoAppDTO
    {
        public CabeceraPartidoAppDTO Cabecera { get; set; }
        public List<EventoAppDTO> Eventos { get; set; } = new List<EventoAppDTO>();
    }

    public class CabeceraPartidoAppDTO
    {
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string NombreFase { get; set; }
        public string NombreEstado { get; set; }

        public int IdEquipoLocal { get; set; }
        public string ClubLocal { get; set; }
        public string LogoLocal { get; set; }
        public int GolesLocal { get; set; }

        public int IdEquipoVisitante { get; set; }
        public string ClubVisitante { get; set; }
        public string LogoVisitante { get; set; }
        public int GolesVisitante { get; set; }
    }

    public class EventoAppDTO
    {
        public int Minuto { get; set; }
        public int IdTipoEvento { get; set; }
        public string NombreJugador { get; set; }
        public int IdEquipo { get; set; }
    }
}