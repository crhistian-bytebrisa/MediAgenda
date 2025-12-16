using MediAgenda.Application.DTOs.Enums;
using System.Collections.Generic;

namespace MediAgenda.Application.DTOs
{
    public class ConsultationDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public PatientSimpleDTO Patient { get; set; }
        public int ReasonId { get; set; }
        public ReasonSimpleDTO Reason { get; set; }
        public int DayAvailableId { get; set; }
        public DayAvailableSimpleDTO DayAvailable { get; set; }
        public string State { get; set; }
        public int Turn { get; set; }
        public int NotesCount { get; set; }
        public int PrescriptionId { get; set; }
        public PrescriptionSimpleDTO Prescription { get; set; }
    }

    public class ConsultationSimpleDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } 
        public string IdentificationPatient { get; set; }
        public int ReasonId { get; set; }
        public string ReasonName { get; set; }
        public int DayAvailableId { get; set; }
        public DateOnly Date { get; set; }
        public string State { get; set; }
        public int Turn { get; set; }
    }

    public class ConsultationCreateDTO
    {
        public int PatientId { get; set; }
        public int ReasonId { get; set; }
        public int DayAvailableId { get; set; }
        public ConsultationStateDTO State { get; set; }
    }

    public class ConsultationUpdateDTO : ConsultationCreateDTO
    {
        public int Id { get; set; }
    }
}
