using System.ComponentModel.DataAnnotations;

namespace DreamsMade.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }


        [Required]
        [Display(Name = "User name")]
        [StringLength(20)]
        public string name { get; set; }


        [Required]
        [StringLength(20)]
        [DataType(DataType.Password)]
        public string password { get; set; }


        public virtual ICollection<Image> images { get; set; }
    }
}
