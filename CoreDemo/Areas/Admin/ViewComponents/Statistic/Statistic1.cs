using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic1: ViewComponent
    {
        BlogManager bm = new BlogManager( new EfBlogRepository() );

        Context c = new Context();

        // Eğer json format için api isteğini httpclient ile yapacaksak Task<> kullanılmalı:

        //public async Task<IViewComponentResult> InvokeAsync()

        // Eğer xml iş görürse ve api isteği XDocument ile yapılacaksa aşağıdaki şekilde devam et:
        public IViewComponentResult Invoke()
        {
            ViewBag.v1 = bm.GetList().Count();
            ViewBag.v2 = c.Contacts.Where( x => x.ContactID == 1 ).Count();
            ViewBag.v3 = c.Comments.Count();

            string myWeatherApi = "fdbec997890c250b5353a4ac8fc1d9d7";

            string connectionForIst = "https://api.openweathermap.org/data/2.5/weather?lat=41.015137&lon=28.979530&appid=" + myWeatherApi + "&units=metric&mode=xml";

            XDocument document = XDocument.Load(connectionForIst);

            ViewBag.v4 = document.Descendants("temperature").ElementAt(0).Attribute("value").Value;

            //string myWeatherApi = "fdbec997890c250b5353a4ac8fc1d9d7";

            //string connectionForIst = "https://api.openweathermap.org/data/2.5/weather?lat=41.015137&lon=28.979530&appid=" + myWeatherApi + "&units=metric";

            //// JSON formatında veriyi almak için HttpClient kullanarak isteği yapıyoruz
            //using (var client = new HttpClient())
            //{
            //    var response = await client.GetStringAsync(connectionForIst);

            //    // JSON formatında gelen veriyi parse etmek için Newtonsoft.Json kullanabiliriz
            //    dynamic weatherData = Newtonsoft.Json.JsonConvert.DeserializeObject(response);

            //    // Sıcaklık bilgisini alıyoruz
            //    ViewBag.v4 = weatherData.main.temp;
            //}

            return View();
        }
    }
}
