
//using System;
//using System.ComponentModel.DataAnnotations;

//namespace HMS.Models
//{
//    public class UsersModel
//    {
//        public int? UserID { get; set; }

//        [Required(ErrorMessage = "Username is required.")]
//        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
//        public string UserName { get; set; }

//        [Required(ErrorMessage = "Password is required.")]
//        [DataType(DataType.Password)]
//        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
//        public string Password { get; set; }

//        [Required(ErrorMessage = "Email is required.")]
//        [EmailAddress(ErrorMessage = "Invalid email address format.")]
//        public string Email { get; set; }

//        [Required(ErrorMessage = "Mobile number is required.")]
//        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
//        public string MobileNo { get; set; }

//        public bool IsActive { get; set; }

//        [DataType(DataType.DateTime)]
//        public DateTime Created { get; set; }

//        [DataType(DataType.DateTime)]
//        public DateTime Modified { get; set; }
//    }
//}

using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models
{
    public class UsersModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username must be less than 100 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, ErrorMessage = "Password must be less than 50 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit mobile number.")]
        public string MobileNo { get; set; }

        public bool IsActive { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Modified { get; set; }

        [Required(ErrorMessage = "Created by User ID is required.")]
        public int UserID_CreatedBy { get; set; }
    }
}
