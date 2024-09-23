using CoreDemo.Models;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICategoryService: IGenericService<Category>
    {
        public int TGetCategoriesCount();

        public List<CategoryWithAverageRaytingViewModel> AllCategoryAveragesWithCategories();
    }
}
