using MediAgenda.Application.DTOs;

namespace MediAgenda.Application.DTOs.Relations
{
    public class HistoryMedicamentDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public PatientSimpleDTO Patient { get; set; }
        public int MedicineId { get; set; }
        public MedicineSimpleDTO Medicine { get; set; }
        public DateOnly StartMedication { get; set; }
        public DateOnly? EndMedication { get; set; }
        public bool IsActive => EndMedication == null || EndMedication >= DateOnly.FromDateTime(DateTime.Today);
    }

    public class HistoryMedicamentSimpleDTO
    {
        public int PatientId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string Format { get; set; }
        public DateOnly StartMedication { get; set; }
        public DateOnly? EndMedication { get; set; }
        public bool IsActive => EndMedication == null || EndMedication >= DateOnly.FromDateTime(DateTime.Today);
    }

    public class HistoryMedicamentCreateDTO
    {
        public int PatientId { get; set; }
        public int MedicineId { get; set; }
        public DateOnly StartMedication { get; set; }
        public DateOnly? EndMedication { get; set; }
    }
}
