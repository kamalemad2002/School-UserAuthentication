using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace School.Models
{
    
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Name")]

        public string Name { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        //[DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]

        [DataType(DataType.Password)]
        //[PasswordPropertyText]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
        [Display(Name = "isAdmin")]
        //public bool isAdmin { get; set; } = true;
        public bool isAdmin { get; set; } 

    }
}
