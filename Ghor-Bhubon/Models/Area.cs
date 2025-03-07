using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ghor_Bhubon.Models
{
    public class Area
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        [StringLength(100)]
        public string Name { get; set; }  // Area name

        [Required]
        public int CityId { get; set; }  // Foreign Key linking to City

        [ForeignKey("CityId")]
        public City City { get; set; }  // Navigation property
    }
}
