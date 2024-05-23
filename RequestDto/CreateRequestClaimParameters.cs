using Kill_hunger.Models;

namespace Kill_hunger.RequestDto
{
    public class CreateRequestClaimParameters
    {
        public string Discription { get; set; } = string.Empty;
        public RequestType RequestClaimType { set; get; }
        public int RequestClaimBye { set; get; }

        public int RequestId { get; set; }
    }
}
