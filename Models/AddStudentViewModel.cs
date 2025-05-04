using System.ComponentModel.DataAnnotations;

namespace School.Models

{

    public class AddStudentViewModel
    {
        [Display(Name = "My Name")]
        [Required(ErrorMessage = "this field is required")]
        public string Name { get; set; }
        [Display(Name = "My Email")]
        [Required(ErrorMessage = "this field is required")]
        [EmailAddress]

        public string Email { get; set; }
        [Display(Name = "My phone")]
        [Required(ErrorMessage = "this field is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]

        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "this field is required")]
        public bool Role { get; set; }
        [Required(ErrorMessage = "this field is required")]
        [Display(Name = "My Governorate")]
        public Governorate governorate { get; set; }
        //public User user {  get; set; }
    }
}
