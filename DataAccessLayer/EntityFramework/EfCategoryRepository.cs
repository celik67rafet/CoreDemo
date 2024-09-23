using CoreDemo.Models;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLayer.EntityFramework
{
    public class EfCategoryRepository : GenericRepository<Category>, ICategoryDal
    {
        public int GetCategoriesCount()
        {
            using ( Context c = new Context() )
            {
                return c.Categories.Count();
            }
        }

        public List<CategoryWithAverageRaytingViewModel> GetCategoriesWithAverages()
        {
            using ( Context c = new Context() ) 
            {
                var categoriesWithScores = c.Categories
                    .Select(category => new CategoryWithAverageRaytingViewModel
                    {
                        Category = category,
                        AverageRayting = category.Blogs.Where( b => b.BlogRayting != null && b.BlogRayting.AverageScore > 0 ).Average( b => b.BlogRayting.AverageScore ) ?? 0
                    });

                return categoriesWithScores.ToList();
            }
        }

    }
}
