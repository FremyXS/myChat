using Pract.Requests;

namespace Pract.Models
{
    public class Account
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }

        public Account() { }
        public Account(AccountRequest accountRequest, byte[] passwordSalt, byte[] passwordHash)
        {
            Email = accountRequest.Email;
            Login = accountRequest.Login;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
        }
    }
}
