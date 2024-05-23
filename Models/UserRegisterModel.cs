namespace Kill_hunger.Models
{
    public class UserRegisterModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsIndivitual { get; set; }
        public bool IsProvider { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public List<IFormFile> FileDetails { get; set; }
        public string  Filetype { get; set; }


    }
}
