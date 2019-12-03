using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Noteloves_server.Data;
using Noteloves_server.Messages.Requests.User;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Noteloves_server.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;

        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAllUser()
        {
            return _context.users;
        }

        public void AddUser(AddUserForm addUserForm)
        {
            User user = new User();

            user.Password = EncodePassword(addUserForm.Password);
            user.Name = addUserForm.Name;
            user.Birthday = addUserForm.BirthDay;
            user.Email = addUserForm.Email;
            user.SyncCode = "";

            _context.users.Add(user);
        }

        public User GetInfomation(int id)
        {
            return _context.users.Find(id);
        }

        public void EidtInfomation(int id, EditUserForm editUserForm)
        {
            var user = _context.users.First(a => a.Id == id);

            user.Name = editUserForm.Name;
            user.Birthday = editUserForm.BirthDay;
            user.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }

        public void ChangePassword(int id, string newPassword)
        {
            var user = _context.users.First(a => a.Id == id);

            user.Password = EncodePassword(newPassword);
            user.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
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

        public string EncodePassword(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes("vImpFshCopbsLMj72tfiTaFYhbkfGg8qZBrY6yZS71A=");

            var generated = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: password,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8));

            return generated;
        }

        public bool UserExistsByEmail(string email)
        {
            return _context.users.Any(e => e.Email == email);
        }

        public bool UserExistsById(int id)
        {
            return _context.users.Any(e => e.Id == id);
        }

        public bool CheckOldPassword(int id, string oldPassword)
        {
            return _context.users.Any(e => e.Id == id && e.Password == EncodePassword(oldPassword));
        }

        public void UpdateSyncCode(int id)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var user = _context.users.First(a => a.Id == id);
            user.SyncCode = new String(stringChars);

            _context.SaveChanges();
        }

        public void EditUserName(int id, string userName)
        {
            var user = _context.users.First(a => a.Id == id);
            user.Name = userName;

            _context.SaveChanges();
        }

        public void EditBirthday(int id, DateTime birthday)
        {
            var user = _context.users.First(a => a.Id == id);
            user.Birthday = birthday;

            _context.SaveChanges();
        }

        public void UpdatePartnerId(int id, int partnerID)
        {
            var user = _context.users.First(a => a.Id == id);
            user.PartnerId = partnerID;

            _context.SaveChanges();
        }
    }
}
