using System.ComponentModel.DataAnnotations;

namespace FormAPI.Dto.Request
{
    public class UpdateCustomer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Gender { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
