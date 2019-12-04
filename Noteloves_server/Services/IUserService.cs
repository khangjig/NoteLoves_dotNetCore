using Noteloves_server.Messages.Requests.User;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Noteloves_server.Services
{
    public interface IUserService
    {
        void AddUser(AddUserForm addUserForm);
        IEnumerable<User> GetAllUser();
        void ChangePassword(int id, string newPassword);
        void EidtInfomation(int id, EditUserForm editUserForm);
        User GetInfomation(int id);
        void UpdateRefreshToken(int id, string refreshToken);
        int GetIdByEmail(string email);
        string GetRefreshToken(string email);
        string EncodePassword(string password);
        bool CheckOldPassword(int id, string oldPassword);
        bool UserExistsById(int id);
        bool UserExistsByEmail(string email);
        void UpdateSyncCode(int id);
        void EditUserName(int id, string userName);
        void EditBirthday(int id, DateTime birthday);
        void UpdatePartnerId(int id, int partnerID);
        void UpdateLoveDay(int id, DateTime loveday);
        int GetIdBySyncCode(string syncCode);
        bool CheckSyncCode(int userId, string syncCode);
        bool CheckSync(int userId);
    }
}
