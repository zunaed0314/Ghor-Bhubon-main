using System;
using System.ComponentModel.DataAnnotations;

namespace Ghor_Bhubon.Models
{
    public enum UserRole
    {
        Landlord = 0,
        Tenant = 1,
        Admin = 2
    }
    
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //[Required]
        //[DataType(DataType.Date)]
        //public DateTime? Birthdate { get; set; } 

        [Required]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
    }
}
