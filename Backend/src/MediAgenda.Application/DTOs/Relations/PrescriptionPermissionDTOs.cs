using System.ComponentModel.DataAnnotations;

namespace MediAgenda.Application.DTOs.Relations
{
    public class PrescriptionPermissionDTO
    {
        public int PrescriptionId { get; set; }
        public PrescriptionSimpleDTO Prescription { get; set; }
        public int PermissionId { get; set; }
        public PermissionSimpleDTO Permission { get; set; }
    }

    public class PrescriptionPermissionSimpleDTO
    {
        public int PrescriptionId { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
    }

    public class PrescriptionPermissionCreateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int PrescriptionId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int PermissionId { get; set; }
    }

    public class PrescriptionPermissionUpdateDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int PrescriptionId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public int PermissionId { get; set; }
    }

}
