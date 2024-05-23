namespace Kill_hunger
{
    public class APIResponse
    {
        public string Status { get; set; } = "Success";
        public string Message { get; set; }  = string.Empty;
        public object Data { get; set; } = null!;
    }
}
