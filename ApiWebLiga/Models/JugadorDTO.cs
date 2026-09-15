namespace ApiWebLiga.Models
{
    public class JugadorDTO
    {
        public string NombreJugador { get; set; }
        public string NroComet { get; set; }
        public string NroCi { get; set; }
        public string FechaNacimientoStr { get; set; }
        public int Edad { get; set; }
        public string FotografiaUrl { get; set; }
        public string NombreClub { get; set; }
        public string LogoUrl { get; set; }
        public int NroCamiseta { get; set; }
    }
}