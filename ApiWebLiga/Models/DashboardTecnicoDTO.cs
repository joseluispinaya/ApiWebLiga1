namespace ApiWebLiga.Models
{
    public class DashboardTecnicoDTO
    {
        // A) Datos del Técnico
        public string NombreTecnico { get; set; }
        public string CargoTecnico { get; set; }

        // B) Datos del Equipo
        public string NombreClub { get; set; }
        public string LogoClub { get; set; }
        public string NombreCategoria { get; set; }
        public string NombreTorneo { get; set; }
        public string NombreSerie { get; set; }

        // C) Estadísticas
        public int TotalJugadores { get; set; }

        // D) Información del Próximo Partido
        public string FechaProximoPartido { get; set; }
        public string HoraProximoPartido { get; set; }
        public string CanchaProximoPartido { get; set; }
        public string NombreRival { get; set; }
    }
}