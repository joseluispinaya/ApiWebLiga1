namespace ApiWebLiga.Models
{
    public class DeudasArbitrajeDTO
    {
        public int IdPartido { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Cancha { get; set; }
        public string NombreFase { get; set; }

        // Datos del Equipo del Técnico
        public int IdEquipo { get; set; }
        public string NombreClub { get; set; }
        public string LogoClub { get; set; }
        public string Condicion { get; set; } // 'Local' o 'Visitante'

        // Datos del Rival
        public string NombreRival { get; set; }
        public string LogoRival { get; set; }

        // Resultado del partido
        public string MarcadorFinal { get; set; }
    }
}