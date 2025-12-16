using System.ComponentModel.DataAnnotations;

namespace MediAgenda.Application.DTOs.Relations
{
    public class PrescriptionMedicineDTO
    {
        public int PrescriptionId { get; set; }
        public PrescriptionSimpleDTO Prescription { get; set; }
        public int MedicineId { get; set; }
        public MedicineSimpleDTO Medicine { get; set; }
        public DateOnly StartDosage { get; set; }
        public DateOnly EndDosage { get; set; }
        public string Instructions { get; set; }
    }

    public class PrescriptionMedicineSimpleDTO
    {
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string MedicineFormat { get; set; }
        public DateOnly StartDosage { get; set; }
        public DateOnly EndDosage { get; set; }
        public string Instructions { get; set; }
    }

    public class PrescriptionMedicineCreateDTO
    {

        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [MinLength(10, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
        public string Instructions { get; set; }
        public DateOnly StartDosage { get; set; }
        public DateOnly EndDosage { get; set; }
    }

    public class PrescriptionMedicineUpdateDTO
    {
        public int PrescriptionId { get; set; }
        public int MedicineId { get; set; }
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener mas de {1} caracteres.")]
        [MinLength(10, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
        public string Instructions { get; set; }
        public DateOnly StartDosage { get; set; }
        public DateOnly EndDosage { get; set; }
    }

}
