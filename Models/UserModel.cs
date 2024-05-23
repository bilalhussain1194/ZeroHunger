using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Kill_hunger.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }

        [AllowNull]
 
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; }

        [AllowNull]
        public string usertoken { get; set; }=string.Empty;
        [AllowNull]
        public string city { get; set; } = string.Empty;
        [AllowNull]
        public string country { get; set; } = string.Empty;


    }
}
