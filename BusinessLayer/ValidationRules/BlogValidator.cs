using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class BlogValidator: AbstractValidator<Blog>
    {
        public BlogValidator()
        {
            RuleFor( x => x.BlogTitle ).NotEmpty().WithMessage("Blog Başlığı Boş Olmamalı");

            RuleFor(x => x.BlogContent).NotEmpty().WithMessage("Blog İçeriği Boş Olmamalı");

            RuleFor(x => x.BlogImage).NotEmpty().WithMessage("Blog Görseli Boş Olmamalı");

            RuleFor(x => x.BlogTitle).MaximumLength(150).WithMessage("Blog Başlığı 150 Karakteri Geçmemeli");

            RuleFor(x => x.BlogTitle).MinimumLength(5).WithMessage("Blog Başlığı En Az 5 Karakter Olmalı");
        }
    }
}
