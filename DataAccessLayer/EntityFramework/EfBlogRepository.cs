using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
	public class EfBlogRepository : GenericRepository<Blog>, IBlogDal
	{
        public int GetBlogsCount()
        {
            using ( var c = new Context() ) 
            {
                return c.Blogs.Count();
            }
        }

        public int GetBlogsCountByWriter(int id)
        {
            using ( var c = new Context() )
            {
                return c.Blogs.Where( x => x.WriterID == id ).Count();
            }
        }

        public List<Blog> GetListWithCategoryAndWriter()
		{
			using (var c = new Context())
			{

				return c.Blogs.Include( x => x.Category ).Include( y => y.Writer ).ToList();
			}

		}

        public List<Blog> GetListWithCategoryByWriter(int id)
        {
			using ( var c = new Context()) 
			{
				return c.Blogs.Include( x => x.Category ).Where( x => x.WriterID == id ).ToList();
			}
        }
    }
}
