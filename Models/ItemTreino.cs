using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAcademia.Models
{
    public class ItemTreino
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TreinoId { get; set; }
        [ForeignKey("TreinoId")]
        public Treino Treino { get; set; } = null!;

        [Required]
        public int ExercicioId { get; set; }
        [ForeignKey("ExercicioId")]
        public Exercicio Exercicio { get; set; } = null!;

        // --- ESTA É A LINHA QUE ESTÁ FALTANDO ---
        [Required]
        public int Series { get; set; } 
        // ----------------------------------------
        
        [Required]
        [StringLength(50)]
        public string Repeticoes { get; set; } = string.Empty;

        [StringLength(50)]
        public string? TempoDescanso { get; set; } 
        
        public string? Observacoes { get; set; }
    }
}