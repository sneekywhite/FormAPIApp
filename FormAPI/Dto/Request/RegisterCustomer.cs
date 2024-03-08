using FormAPI.Service;
using System.ComponentModel.DataAnnotations;

namespace FormAPI.Dto.Request
{
    public class RegisterCustomer
    {
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [DataType(DataType.Text)]
        public string Address { get; set; }
        [Required]
        [StringLength(20)]
        [Phone(ErrorMessage ="kindly input valid number")]
        public string PhoneNo { get; set; }
        
        public string Gender { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; } 
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
}
