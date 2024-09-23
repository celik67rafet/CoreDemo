using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class CategoryValidator: AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor( x => x.CategoryName ).NotEmpty().WithMessage("Kategori Adı Boş Olmamalı!");  
            RuleFor( x => x.CategoryDescription ).NotEmpty().WithMessage("Kategori Açıklaması Boş Olmamalı!");  
            RuleFor( x => x.CategoryName ).MaximumLength( 50 ).WithMessage("Kategori Adı En Fazla 50 Karakter Olmalı!");  
            RuleFor( x => x.CategoryName ).MinimumLength( 2 ).WithMessage("Kategori Adı En Az 2 Karakter Olmalı!");  
        }
    }
}
