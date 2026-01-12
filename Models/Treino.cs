using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Precisamos disso para a lista

namespace SistemaAcademia.Models
{
    public class Treino
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do treino é obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        // O '?' torna o campo opcional
        public string? Observacoes { get; set; }

        // Isso cria o relacionamento: "Um Treino pode ter VÁRIOS ItensTreino"
        // (Isso será o Módulo 2.3, mas já vamos deixar pronto)
        public ICollection<ItemTreino> ItensTreino { get; set; } = new List<ItemTreino>();
    }
}