namespace Pract.Dto
{
    public class AccountDto
    {
        public string Token { get; set; }
        public string Login { get; set; }

        public AccountDto(string token, string login)
        {
            Token = token;
            Login = login;
        }
    }
}
