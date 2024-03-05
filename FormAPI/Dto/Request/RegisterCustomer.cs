using FormAPI.Service;
using System.ComponentModel.DataAnnotations;

namespace FormAPI.Dto.Request
{
    public class RegisterCustomer
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        [Required]
        [StringLength(20)]
        [Phone(ErrorMessage ="kindly input valid number")]
        public string PhoneNo { get; set; }
        public string Gender { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; } 
    }
}
