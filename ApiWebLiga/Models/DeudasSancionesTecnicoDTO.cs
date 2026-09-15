namespace ApiWebLiga.Models
{
    public class DeudasSancionesTecnicoDTO
    {
        public int IdPartido { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Cancha { get; set; }
        public string NombreFase { get; set; }
        public int IdEquipo { get; set; }
        public string NombreRival { get; set; }
        public int NroSanciones { get; set; }
        public decimal DeudaTotal { get; set; }
    }
}