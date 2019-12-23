using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noteloves_server.Data;
using Noteloves_server.JWTProvider.Services;
using Noteloves_server.Messages.Responses;
using Noteloves_server.Services;
using Noteloves_server.Services.Imp;

namespace Noteloves_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private INotificationService _notificationService;
        private IUserService _userService;
        private IJWTService _jWTService;

        public NotificationController(DatabaseContext context, INotificationService notificationService, IUserService userService, IJWTService jWTService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
            _jWTService = jWTService;
        }

        // POST : api/notification
        [HttpPost]
        public async Task<IActionResult> CreatedNotification([FromForm]string syncCodePartner)
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not exist!"));
            }
            
            if (_userService.GetIdBySyncCode(syncCodePartner) == -1)
            {
                return BadRequest(new Response("404", "Can not find the User!"));
            }
            
            if (_userService.CheckSync(userId))
            {
                return BadRequest(new Response("400", "You can not Sync!"));
            }
            
            if (_userService.CheckSync(_userService.GetIdBySyncCode(syncCodePartner)))
            {
                return BadRequest(new Response("400", "Partner has be Synchronized!"));
            }
            
            if (!_userService.CheckSyncCode(userId, syncCodePartner))
            {
                return BadRequest(new Response("404", "That's your Sync code, Dude!"));
            }

            _notificationService.CreatedNotification(userId, _userService.GetIdBySyncCode(syncCodePartner));

            await _context.SaveChangesAsync();

            return Ok(new Response("200", "Successfully!"));
        }

        // GET : api/notification
        [HttpGet]
        public IActionResult GetNotification()
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not exist!"));
            }

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not exist!"));
            }

            return Ok(new DataResponse("200", _notificationService.GetNotification(userId), "Successfully!"));
        }

        //POST : api/notification/actived 
        [HttpPost]
        [Route("actived")]
        public IActionResult SyncActive([FromForm] int notificationID )
        {
            var partnerId = GetIdByToken(this);

            if (!_userService.UserExistsById(partnerId))
            {
                return NotFound(new Response("404", "User not exist!"));
            }

            if (!_notificationService.CheckNotification(partnerId, notificationID) 
                || _userService.CheckSync(partnerId)
                || _userService.CheckSync(_notificationService.GetUserIDByNotificationID(notificationID)))
            {
                return BadRequest(new Response("400", "The user can not Sync!"));
            }

            _notificationService.SyncActived(partnerId, notificationID);

            return Ok(new Response("200", "Successfully!"));
        }

        //POST : api/notification/deny 
        [HttpPost]
        [Route("deny")]
        public IActionResult SyncDeny([FromForm] int notificationID)
        {
            var partnerId = GetIdByToken(this);

            if (!_userService.UserExistsById(partnerId))
            {
                return NotFound(new Response("404", "User not exist!"));
            }

            if (!_notificationService.CheckNotification(partnerId, notificationID))
            {
                return BadRequest(new Response("400", "The Notification not exist!"));
            }

            _notificationService.SyncDeny(partnerId, notificationID);

            return Ok(new Response("200", "Successfully!"));
        }

        //POST : api/notification/cancelSync 
        [HttpPost]
        [Route("cancelSync")]
        public IActionResult CancelSync([FromForm] int partnerID)
        {
            var userID = GetIdByToken(this);

            if (!_userService.UserExistsById(userID))
            {
                return NotFound(new Response("404", "User not exist!"));
            }

            if (!_userService.UserExistsById(partnerID))
            {
                return NotFound(new Response("404", "Partner not exist!"));
            }

            if (!_notificationService.CheckSyncCouple(userID,partnerID))
            {
                return NotFound(new Response("404", "You and his/her not sync!"));
            }

            _notificationService.CancelSync(userID, partnerID);

            return Ok(new Response("200", "Successfully!"));
        }

        private int GetIdByToken(NotificationController notificationController)
        {
            var authorization = Request.Headers["Authorization"];
            var accessToken = authorization.ToString().Replace("Bearer ", "");
            return _jWTService.GetIdByToken(accessToken);
        }
    }
}