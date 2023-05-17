namespace Pract.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
