using System;
using System.ComponentModel.DataAnnotations;

namespace Ghor_Bhubon.Models
{
    public enum UserRole
    {
        Tenant = 0,
        Landlord = 1,
        Admin = 2
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
