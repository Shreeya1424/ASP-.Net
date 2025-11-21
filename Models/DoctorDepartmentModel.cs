using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class DoctorDepartmentModel
    {
        public int DoctorDepartmentID { get; set; }

        [Required(ErrorMessage = "Please select a doctor")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a doctor")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Please select a department")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Please select a user")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a user")]
        public int UserID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }

        //[Required(ErrorMessage = "User ID is required.")]
        //[Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive number.")]
        //public int UserID { get; set; }
    }
}
