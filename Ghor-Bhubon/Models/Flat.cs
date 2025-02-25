using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ghor_Bhubon.Models
{
    public class Flat
    {
        [Key]
        public int FlatID { get; set; }

        public int UserID { get; set; }

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

        public string Availability { get; set; } = "Available";

        public string? ImagePaths { get; set; } // Store multiple image paths as a comma-separated string

        [NotMapped]
        public List<string> ImagePathList
        {
            get => string.IsNullOrEmpty(ImagePaths) ? new List<string>() : ImagePaths.Split(',').ToList();
        }
    }
}
