using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TCC_Projeto.Models.Etiqueta
{   
    public class FitaViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int Id { get; set; }

        [Required]
        public required int impressao_id { get; set; }

        [Required]
        public required int quantidade { get; set; }

        [Required]
        public required int largura { get; set; }
    }
}
