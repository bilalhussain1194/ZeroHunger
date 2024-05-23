using Kill_hunger.Models;

namespace Kill_hunger.RequestDto
{
    public class CreateRequestParameters
    {
        public RequestType RequestType { get; set; }
        public string Discription { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
