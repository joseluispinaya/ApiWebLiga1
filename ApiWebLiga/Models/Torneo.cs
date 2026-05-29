namespace ApiWebLiga.Models
{
    public class Torneo
    {
        public int IdTorneo { get; set; }
        public string NombreTorneo { get; set; }
        public int Gestion { get; set; }
        public int PuntosVictoriaLocal { get; set; }
        public int PuntosVictoriaVisitante { get; set; }
        public int PuntosEmpateLocal { get; set; }
        public int PuntosEmpateVisitante { get; set; }
        public bool Estado { get; set; }
    }
}