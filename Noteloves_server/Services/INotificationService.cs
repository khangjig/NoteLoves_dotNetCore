using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services
{
    public interface INotificationService
    {
        void CreatedNotification(int userID, int partnerId);
        List<Notification> GetNotification(int userID);
        void SyncActived(int partnerID, int notificationID);
        void SyncDeny(int partnerID, int notificationID);
        void CancelSync(int userID, int partnerId);
        bool CheckSyncCouple(int userID, int partnerId);
        bool CheckNotification(int partnerID, int notificationID);
        int GetUserIDByNotificationID(int notificationID);
        int GetPartnerIDByNotificationID(int notificationID);
    }
}
