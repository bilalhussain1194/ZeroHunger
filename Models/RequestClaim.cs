using System.ComponentModel.DataAnnotations;

namespace Kill_hunger.Models
{
    public class RequestClaim
    {
        [Key]
        public int Id { get; set; }
        public string Discription { get; set; } = string.Empty;
        public RequestType RequestClaimType { set; get; }
        public int RequestClaimBye { set; get; }

        public int RequestId { get; set; }
        public Request Request { get; set; } = null!;
    }

}
