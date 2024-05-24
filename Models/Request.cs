using System.ComponentModel.DataAnnotations;

namespace Kill_hunger.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }
        public RequestType RequestType { get; set; }
        public string Discription { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int UserId { get; set; }
        public bool IsClaimed { get; set; }
        public bool IsDelete { get; set; }
        public bool IsClose { get; set; }
        public string? CloseReason { get; set; }

        public List<RequestClaim> RequestClaim { get; set; } = [];
    }
}
