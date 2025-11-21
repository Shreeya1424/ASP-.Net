using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class AppointmentModel
    {
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Appointment date is required.")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Appointment status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string AppointmentStatus { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [StringLength(300, ErrorMessage = "Special remarks cannot exceed 300 characters.")]
        public string SpecialRemarks { get; set; }

        public bool IsActive { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }

        [Range(0, 100000, ErrorMessage = "Amount must be a valid number.")]
        public decimal?  TotalConsultedAmount { get; set; }
    }
}
