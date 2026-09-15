namespace ApiWebLiga.Models
{
    public class PartidosDTO
    {
        public int IdPartido { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Cancha { get; set; }

        // Datos Local
        public string ClubLocal { get; set; }
        public string LogoLocal { get; set; }

        // Datos Visitante
        public string ClubVisitante { get; set; }
        public string LogoVisitante { get; set; }
    }
}