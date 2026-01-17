using System.ComponentModel.DataAnnotations;

namespace MediAgenda.Application.DTOs.Relations
{

    public class PrescriptionAnalysisDTO
    {
        public int PrescriptionId { get; set; }
        public PrescriptionSimpleDTO Prescription { get; set; }
        public int AnalysisId { get; set; }
        public AnalysisSimpleDTO Analysis { get; set; }
        public string Recomendations { get; set; }
    }

    public class PrescriptionAnalysisSimpleDTO
    {
        public int PrescriptionId { get; set; }
        public int AnalysisId { get; set; }
        public string AnalysisName { get; set; }
        public string Recomendations { get; set; }
    }

    public class PrescriptionAnalysisCreateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int PrescriptionId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int AnalysisId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(300, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [MinLength(10, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
        public string Recomendations { get; set; }
    }

    public class PrescriptionAnalysisUpdateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int PrescriptionId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int AnalysisId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(300, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [MinLength(10, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
        public string Recomendations { get; set; }
    }
}

