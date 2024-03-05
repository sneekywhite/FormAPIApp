using System.ComponentModel.DataAnnotations;

namespace FormAPI.Models
{
    public class refreshToken
    {
        [Key]
        public string userid { get; set; }
        public string tokenid { get; set;}
        public string Token { get; set; }
    }
}
