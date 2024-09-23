using EntityLayer.Concrete;

namespace CoreDemo.Models
{
    public class MessageNotificationViewModel
    {
        public List<Message2> Messages { get; set; }
        public AppUser User { get; set; }
    }
}
