using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaAcademia.Models
{
    public class RegistroItem
    {
        [Key]
        public int Id { get; set; }

        // --- Link para o "Diário" (o dia do treino) ---
        [Required]
        public int RegistroTreinoId { get; set; }
        [ForeignKey("RegistroTreinoId")]
        public RegistroTreino RegistroTreino { get; set; } = null!;

        // --- Link para o Exercício (qual exercício foi feito) ---
        [Required]
        public int ExercicioId { get; set; }
        [ForeignKey("ExercicioId")]
        public Exercicio Exercicio { get; set; } = null!;

        // --- O dado mais importante: a carga (peso) usada ---
        [Required(ErrorMessage = "Informe a carga utilizada")]
        public string Carga { get; set; } = string.Empty; // Ex: "20kg"

        // Opcionais: Para anotar se você fez algo diferente do planejado
        public string? SeriesRealizadas { get; set; }     // Ex: "3"
        public string? RepeticoesRealizadas { get; set; } // Ex: "12, 10, 8"
    }
}