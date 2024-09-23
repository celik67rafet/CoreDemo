using BusinessLayer.Abstract;
using CoreDemo.Models;
using DataAccessLayer.Abstract;
using DataAccessLayer.EntityFramework;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {

        //EfCategoryRepository efCategoryRepository; bunu kullanırsak entityframework'e bağımlı kalırız

        ICategoryDal _categoryDal;

        public List<CategoryWithAverageRaytingViewModel> AllCategoryAveragesWithCategories() 
        {
            return _categoryDal.GetCategoriesWithAverages();
        }

        public CategoryManager( ICategoryDal categoryDal )
        {
            _categoryDal = categoryDal;
        }
        public List<Category> GetList()
        {
            return _categoryDal.GetListAll();
        }

        public Category TGetById(int id)
        {
            return _categoryDal.GetByID( id );
        }

        public void TAdd(Category t)
        {
            _categoryDal.Insert( t );
        }

        public void TDelete(Category t)
        {
            _categoryDal.Delete( t );
        }

        public void TUpdate(Category t)
        {
            _categoryDal.Update( t );
        }
       
        public int TGetCategoriesCount()
        {
            return _categoryDal.GetCategoriesCount();
        }
    }
}
