using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Notification
{
    public class NotificationList: ViewComponent
    {
        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
