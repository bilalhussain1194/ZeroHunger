using Kill_hunger.Models;

namespace Kill_hunger.RequestDto
{
    public class CloseRequestParameters
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public bool IsClose { get; set; }
        public string? CloseReason { get; set; }
    }
}
