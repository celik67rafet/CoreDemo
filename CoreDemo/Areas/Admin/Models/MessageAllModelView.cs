using EntityLayer.Concrete;

namespace CoreDemo.Areas.Admin.Models
{
    public class MessageAllModelView
    {
        public List<Message2> Inbox { get; set; }
        public List<Message2> SendBox { get; set; }
        public int TotalInboxPages { get; set; } // Toplam gelen kutusu sayfaları
        public int TotalSendBoxPages { get; set; } // Toplam giden kutusu sayfaları
    }
}
