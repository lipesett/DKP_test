namespace teste_api.Models.Entites
{
    public class Aluno
    {
        public Aluno()
        {
            Materia = new List<Materia>();
        }
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public string? Telefone { get; set; }
        public ICollection<Materia>? Materia { get; set; }
    }
}