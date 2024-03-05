using System.ComponentModel.DataAnnotations;

namespace FormAPI.Dto.Request
{
    public class LoginCustomer {

        [EmailAddress]
        [Required]
        public string email { get; set; }
        [Required]
     
        public string password { get; set; }
    }
}
