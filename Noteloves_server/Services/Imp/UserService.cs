using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Noteloves_server.Data;
using Noteloves_server.Messages.Requests;
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
            user.Sex = addUserForm.Sex;
            user.Birthday = addUserForm.BirthDay;
            user.Email = addUserForm.Email;

            _context.users.Add(user);
        }

        public User GetInfomation(int id)
        {
            return _context.users.Find(id);
        }

        public void EidtInfomation(EditUserForm editUserForm)
        {
            var user = _context.users.First(a => a.Id == editUserForm.Id);

            user.Name = editUserForm.Name;
            user.Sex = editUserForm.Sex;
            user.Birthday = editUserForm.BirthDay;

            _context.SaveChanges();
        }

        public void ChangePassword(int id, string newPassword)
        {
            var user = _context.users.First(a => a.Id == id);

            user.Password = EncodePassword(newPassword);
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
    }
}
