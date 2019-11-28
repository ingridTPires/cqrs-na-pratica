namespace API.Models
{
    public sealed class AlunoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public string Curso1 { get; set; }
        public string Curso1Grade { get; set; }
        public string Curso1ComentarioDesincricao { get; set; }
        public int? Curso1Creditos { get; set; }
        public string Curso2 { get; set; }
        public string Curso2Grade { get; set; }
        public string Curso2ComentarioDesincricao { get; set; }
        public int? Curso2Creditos { get; set; }
    }
}
