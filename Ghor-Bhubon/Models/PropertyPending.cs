using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ghor_Bhubon.Models
{
    public class PropertyPending
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public decimal Rent { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int NumberOfRooms { get; set; }

        [Required]
        public int NumberOfBathrooms { get; set; }

        public string Availability { get; set; } = "Pending Approval";

        public string? ImagePaths { get; set; } // Store multiple image paths as a comma-separated string

        public string? PdfFilePath { get; set; } // Store the uploaded PDF file path
        public double Longitude { get; internal set; }
        public double Latitude { get; internal set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Area { get; set; }

        [Required]
        public DateOnly AvailableFrom { get; set; }
    }

}
