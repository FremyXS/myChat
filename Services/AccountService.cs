using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pract.Database;
using Pract.Dto;
using Pract.Models;
using Pract.Requests;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Pract.Services
{
    public class AccountService
    {
        private readonly ChatContext _chatContext;
        public AccountService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<Account> CreateAccount(AccountRequest accountRequest)
        {
            var account = await _chatContext.Accounts
                .FirstOrDefaultAsync(x => x.Email.Equals(accountRequest.Email) || x.Login.Equals(accountRequest.Login));

            if(account != null)
            {
                throw new Exception("Email or Login are live");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(accountRequest.Password, out passwordHash, out passwordSalt);

            var newAccount = await _chatContext.Accounts.AddAsync(
                new Account(accountRequest, passwordSalt, passwordHash)
            );

            await _chatContext.SaveChangesAsync();

            return newAccount.Entity;
        }

        public async Task<AccountDto> Authenticate(AccountRequest accountRequest)
        {
            var account = await _chatContext.Accounts.FirstOrDefaultAsync(x => x.Email == accountRequest.Email || x.Login == accountRequest.Login);

            if (account == null)
            {
                throw new Exception("Email or Login are not live");
            }

            if (!VerifyPasswordHash(accountRequest.Password, account.PasswordHash, account.PasswordSalt))
            {
                throw new Exception("Password Incorrect");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, account.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new AccountDto(tokenString, account.Login);
        }
        public async void RemoveRefreshTokens(int accountId)
        {
            var refreshTokens = _chatContext.RefreshTokens.Where(x => x.AccountId == accountId);
            _chatContext.RefreshTokens.RemoveRange(refreshTokens);
            await _chatContext.SaveChangesAsync();
        }

        public async Task<Account> GetAccount(int accountId)
        {
            var account = await _chatContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId);

            if (account == null)
            {
                throw new Exception("Email or Login are not live");
            }

            return account;

        }
        //public string RefreshToken(int userId, string refreshToken)
        //{
        //    var user = _context.Users.Find(userId);
        //    var rt = _context.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.UserId == userId);

        //    if (rt == null)
        //        throw new AppException("Refresh token not found");

        //    // удаляем старый RefreshToken
        //    _context.RefreshTokens.Remove(rt);

        //    // создаем новый RefreshToken
        //    var newRefreshToken = RandomString(32);
        //    var newRt = new RefreshToken
        //    {
        //        UserId = userId,
        //        Token = newRefreshToken,
        //        Expires = DateTime.UtcNow.AddDays(7),
        //    };
        //    _context.RefreshTokens.Add(newRt);

        //    // сохраняем изменения в базе данных
        //    _context.SaveChanges();

        //    // создаем новый AccessToken
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //    //    Subject = new ClaimsIdentity(new Claim[]
        //    //    {
        //    //new Claim(ClaimTypes.Name, user.Id.ToString())
        //    //    }),
        //        Expires = DateTime.UtcNow.AddMinutes(15),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var accessToken = tokenHandler.WriteToken(token);

        //    return accessToken;
        //}

        //private static string RandomString(int length)
        //{
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //    return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        //}

        // приватные методы для работы с хешем пароля
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordSalt");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
