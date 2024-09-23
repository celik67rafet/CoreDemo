using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// JWT CONFIG START ===============================================================================

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme ).AddJwtBearer( x =>
{

	x.RequireHttpsMetadata = false; // geliştirme ortamında https gereksinimini kaldırmak için false...
	x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
	{
		ValidIssuer = "http://localhost",
		ValidAudience = "http://localhost",
		IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes("aspnetcore_projectcamp_from_rafet_as_junior!") ),
		ValidateIssuerSigningKey = true,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero,

	};

} );

// JWT CONFIG END ===============================================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
