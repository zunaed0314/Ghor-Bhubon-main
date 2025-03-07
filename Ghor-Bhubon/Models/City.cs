using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ghor_Bhubon.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        [StringLength(100)]
        public string Name { get; set; }  // City name

        [Required]
        public List<Area> Areas { get; set; }  // Navigation property (One-to-Many)
    }
}
