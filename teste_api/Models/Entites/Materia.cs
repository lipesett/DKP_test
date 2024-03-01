namespace teste_api.Models.Entites
{
    public class Materia
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Periodo { get; set; }
        public required string Tutor { get; set; }
    }
}