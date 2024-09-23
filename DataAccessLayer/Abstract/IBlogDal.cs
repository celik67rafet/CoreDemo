using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IBlogDal: IGenericDal<Blog>
    {
        List<Blog> GetListWithCategoryAndWriter();

        List<Blog> GetListWithCategoryByWriter( int id );

        public int GetBlogsCount();

        public int GetBlogsCountByWriter(int id);
    }
}
