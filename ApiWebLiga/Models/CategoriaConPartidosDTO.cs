namespace ApiWebLiga.Models
{
    public class CategoriaConPartidosDTO
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public string Genero { get; set; }
        public int? EdadMaxima { get; set; } // int? soporta los valores NULL de la BD
        public int CantidadPartidosProgramados { get; set; }
    }
}