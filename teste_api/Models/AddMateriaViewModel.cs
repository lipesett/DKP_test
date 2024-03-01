namespace teste_api.Models
{
    public class AddMateriaViewModel
    {
        public required string Nome { get; set; }
        public required string Periodo { get; set; }
        public string? Tutor { get; set; }
    }
}
