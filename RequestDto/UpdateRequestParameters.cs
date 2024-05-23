using Kill_hunger.Models;

namespace Kill_hunger.RequestDto
{
    public class UpdateRequestParameters
    {
        public int Id { get; set; }
        public RequestType RequestType { get; set; }
        public string Discription { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsDelete { get; set; }
    }
}
