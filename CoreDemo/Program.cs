using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
  Bu kısmı ben ekledim, yerleşik doğrulamayı kapatmak için, bunu kapatmazsak kendi doğrulama mesajlarımız görünmez.
 */

builder.Services.AddControllersWithViews().AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<WriterValidator>();
    fv.DisableDataAnnotationsValidation = true; // ASP.NET Core'un yerleşik doğrulama kurallarını devre dışı bırakma
});

// Identity Kütphanesi ekledikten sonra RegisterUser sayfası için EKLENDİ ============================
builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<AppUser,AppRole>(
	x =>
	{
		x.Password.RequireUppercase = false; //password için gerekli require düzenlemeleri
		x.Password.RequireNonAlphanumeric = false;
	}
	).AddEntityFrameworkStores<Context>();
// Identity Kütphanesi ekledikten sonra RegisterUser sayfası için EKLENDİ - BİTİŞ ============================


// Proje Bazında Authorize ekledik START ------

builder.Services.AddMvc(config =>
{
	var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
	config.Filters.Add(new AuthorizeFilter(policy));
});

// ERİŞİM İZNİ OLMAYAN SAYFALARDAN LOGİNE YÖNLENDİRME START =======

builder.Services.AddAuthentication(
	
		CookieAuthenticationDefaults.AuthenticationScheme
	)
	.AddCookie( x =>
	{
		x.LoginPath = "/Login/Index";
	} );

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Login/Index"; // Identity login yönlendirmesi buraya yapılacak
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // oturum süresi ayarlanır
	options.SlidingExpiration = true; // kullanıcı aktifse oturum uzatılır
	options.AccessDeniedPath = new PathString("/Home/AccessDenied");
});

// ERİŞİM İZNİ OLMAYAN SAYFALARDAN LOGİNE YÖNLENDİRME END ========

// Proje Bazında Authorize ekledik END ---------

builder.Services.AddSession(); // Session ekledik... // [Login Authentication ekledik buna artık gerek yok...(işe yaramadı)]

// Not: Normalde videoda AddSession() kaldırıyor ve çalışmaya devam ediyor ancak biz kaldırınca açılmıyor, o yuzden kalacak.



/*
 
	Yerleşik Doğrulama kapatılmasının bittiği yer...
 
 */

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage(); // Hataları göstermek için kullanılır
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error1","?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // session kullanmayı ekledik...

app.UseAuthentication(); // auth kullanması için koyduk

app.UseAuthorization();

// Aslında UseEndpoints'in başlangıcı burası olmalı....
/* Area kullanımında UseEndpoints kullanılır. Area'lar default olarak UI kök dizinde Areas klasörü
 * ile tanımlanır ve Areas üzerine ekle denince Area eklenebilir. Default olarak
 Controller, Data,Models,Views klasörleri gelir. */

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(

            name: "areas",
            pattern: "{area:exists}/{controller=Default}/{action=Index}/{id?}"

        );

    endpoints.MapControllerRoute(

            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"

        );
});

// Aslında UseEndpoints'in bitişi burası olmalı....

app.Run();
