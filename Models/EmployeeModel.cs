using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class EmployeeModel
    {
        public int? EmployeeID { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public DateTime DOB { get; set; }


        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public string Jobtitle { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public byte[] ProfileImage { get; set; }
        public string ImageFileName { get; set; }
        public string ImageContentType { get; set; }

        // Binding from the form (not mapped in DB)
        [NotMapped]
        public IFormFile ProfileImageFile { get; set; }

        // Optional flag to remove existing image
        public bool RemoveImage { get; set; }

        // Helper to use in the view (computed)
        [NotMapped]
        public string ImageBase64
        {
            get
            {
                if (ProfileImage == null || string.IsNullOrEmpty(ImageContentType)) return null;
                return $"data:{ImageContentType};base64,{Convert.ToBase64String(ProfileImage)}";
            }
        }

    }
}
