namespace ApiWebLiga.Models
{
    public class PlantillaJugadorDTO
    {
        public int IdJugador { get; set; }
        public int Dorsal { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CI { get; set; }
        public string FotografiaUrl { get; set; }
        public bool Activo { get; set; }
    }
}