using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using ELog.Web.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ELog.Web.Host.Controllers
{
    public class HomeController : PMMSControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ILogger<HomeController> _logger;

        public HomeController(INotificationPublisher notificationPublisher, ILogger<HomeController> logger)
        {
            _notificationPublisher = notificationPublisher;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //redirect
            var redirectResult = Redirect("/swagger");
            return redirectResult;
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message">Notifiation Message</param>
        /// <returns>Return Message</returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }


    }
}