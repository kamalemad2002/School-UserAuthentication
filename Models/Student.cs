using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public enum Governorate
    {
       Assiut,
       Aswan,
       Minia,
       Alex,
       Cairo,
    }
    [Table("kamal")]
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{ get; set; }
        [Display(Name="My Name")]
        [Required(ErrorMessage = "this field is required")]
        public string Name{ get; set; }
        [Display(Name = "My Email")]
        [Required(ErrorMessage = "this field is required")]
        public string Email{ get; set; }
        [Display(Name = "My phone")]
        [Required(ErrorMessage = "this field is required")]
        public string? PhoneNumber{ get; set; }
        //[Required(ErrorMessage = "this field is required")]
        //public bool Role{ get; set; }
        [Required(ErrorMessage = "this field is required")]
        [Display(Name="My Governorate")]
        public Governorate governorate{ get; set; }
        //public User? user { get; set; }
    }
}
