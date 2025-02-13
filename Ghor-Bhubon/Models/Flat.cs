// Models/Flat.cs
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        //public List<string> ImagePaths { get; set; } = new List<string>(); 
    }
}
