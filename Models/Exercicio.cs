using System.ComponentModel.DataAnnotations;

namespace SistemaAcademia.Models
{
    public class Exercicio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do exercício é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O grupo muscular é obrigatório")]
        [StringLength(50)]
        public string GrupoMuscular { get; set; } = string.Empty;

        public string? Descricao { get; set; }        // O '?' torna o campo opcional

    }
}