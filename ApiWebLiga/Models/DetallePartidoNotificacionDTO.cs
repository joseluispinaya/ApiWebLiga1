namespace ApiWebLiga.Models
{
    public class DetallePartidoNotificacionDTO
    {
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Cancha { get; set; }
        public string NombreFase { get; set; }

        public string ClubLocal { get; set; }
        public string LogoLocal { get; set; }

        public string ClubVisitante { get; set; }
        public string LogoVisitante { get; set; }
    }
}