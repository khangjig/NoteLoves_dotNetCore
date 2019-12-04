using Noteloves_server.Data;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services.Imp
{
    public class NotificationService : INotificationService
    {
        private readonly DatabaseContext _context;

        public NotificationService(DatabaseContext context)
        {
            _context = context;
        }

        public void CreatedNotification(int userID, int partnerId)
        {
            Notification notification = new Notification();
            notification.UserId = userID;
            notification.PartnerId = partnerId;
            notification.Status = true;
            notification.Title = "SyncData";
            notification.Content = "c-o-n-t-e-n-t";

            _context.notifications.Add(notification);
        }

        public List<Notification> GetNotification(int userID)
        {
            var notifi = _context.notifications.Where(x => x.PartnerId == userID && x.Status == true).ToList();
            return notifi;
        }

        public void SyncActived(int partnerID, int notificationID)
        {
            var users = _context.users.First(x => x.Id == GetUserIDByNotificationID(notificationID));
            users.PartnerId = partnerID;

            var partner = _context.users.First(x => x.Id == partnerID);
            partner.PartnerId = GetUserIDByNotificationID(notificationID);

            var noti = _context.notifications.First(x => x.Id == notificationID);
            noti.Status = false;

            _context.SaveChanges();
        }

        public bool CheckNotification(int partnerID, int notificationID)
        {
            var num = _context.notifications.Where(x => x.PartnerId == partnerID && x.Id == notificationID && x.Status == true).Count();
            if (num > 0) 
            {
                return true;
            }
            return false;
        }

        public int GetUserIDByNotificationID(int notificationID)
        {
            return _context.notifications.First(x => x.Id == notificationID).UserId;
        }

        public int GetPartnerIDByNotificationID(int notificationID)
        {
            return _context.notifications.First(x => x.Id == notificationID).PartnerId;
        }
    }
}
