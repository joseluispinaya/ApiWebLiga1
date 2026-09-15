namespace ApiWebLiga.Models
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }
        public int IdAcceso { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CI { get; set; }
        public string Correo { get; set; }
        public string ClaveHash { get; set; }
    }
}