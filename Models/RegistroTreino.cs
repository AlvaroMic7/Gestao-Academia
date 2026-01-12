using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SistemaAcademia.Models
{
    public class RegistroTreino
    {
        [Key]
        public int Id { get; set; }

        // --- Chave estrangeira para o Treino (A "Ficha") ---
        // Para sabermos qual foi a ficha base que o usuário fez
        [Required]
        public int TreinoId { get; set; }
        [ForeignKey("TreinoId")]
        public Treino Treino { get; set; } = null!; // Relacionamento: 1 Registro aponta para 1 Ficha

        // --- Data ---
        [Required]
        [DataType(DataType.Date)] // Isso garante que será apenas a data
        public DateTime DataTreino { get; set; }

        public string? Observacoes { get; set; } // Ex: "Academia estava cheia"

        // --- Relacionamento ---
        // Um Registro de Treino (diário) terá vários "Registros de Itens" (os pesos)
        public ICollection<RegistroItem> ItensRegistrados { get; set; } = new List<RegistroItem>();
    }
}