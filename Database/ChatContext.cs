using Microsoft.EntityFrameworkCore;
using Pract.Models;

namespace Pract.Database
{
    public class ChatContext : DbContext
    {
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages{ get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ChatContext(DbContextOptions<ChatContext> options): base(options)
        {

        }

    }
}
