namespace ApiWebLiga.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public char Genero { get; set; }

        public int EdadMaxima { get; set; }
        public bool Estado { get; set; }
    }
}