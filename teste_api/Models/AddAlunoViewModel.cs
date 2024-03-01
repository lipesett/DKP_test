using teste_api.Models.Entites;

namespace teste_api.Models
{
    public class AddAlunoViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string? Telefone { get; set; }
        public ICollection<Materia>? ListaMaterias { get; set; }
        public List<CheckBoxOption> CheckBoxes { get; set; }
        public List<int> NomeUnico { get; set; }
    }    
}
