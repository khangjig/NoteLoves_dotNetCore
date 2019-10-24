using Noteloves_server.Data;
using System.Linq;

namespace Noteloves_server.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;

        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public void UpdateRefreshToken(int id, string refreshToken)
        {
            var user = _context.users.First(a => a.Id == id);
            user.RefreshToken = refreshToken;
            _context.SaveChanges();
        }

        public int GetIdByEmail(string email)
        {
            var user = _context.users.First(a => a.Email == email);
            return user.Id;
        }

        public string GetRefreshToken(string email)
        {
            var user = _context.users.First(a => a.Email == email);
            return user.RefreshToken;
        }
    }
}
