namespace Kill_hunger.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public virtual List<FileDetails> FileDetails { get; set; }


    }
}
