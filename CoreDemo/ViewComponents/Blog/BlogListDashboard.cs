using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.ViewComponents.Blog
{
    public class BlogListDashboard: ViewComponent
    {
        BlogManager bm = new BlogManager( new EfBlogRepository() );

        WriterManager wm = new WriterManager( new EfWriterRepository() );

        public IViewComponentResult Invoke()
        {
            var values = bm.GetLast10Blog();

            return View( values );
        }
    }
}
