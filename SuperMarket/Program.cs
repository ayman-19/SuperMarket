
using DB_Core.Models;
using DB_Core.Services;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Impelements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuperMarket.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuperMarket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

            builder.Services.AddHttpClient();
            builder.Services.AddDbContext<SuperMarketContext>(
                o => o.UseSqlServer(
                builder.Configuration.GetConnectionString("myconnection")));
            builder.Services.AddControllers();
            builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<SuperMarketContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<IAccountServices, AccountServices>();
            builder.Services.AddScoped<UserManager<User>>();
            builder.Services.AddScoped<RoleManager<IdentityRole>>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddSwaggerGen(o=>
            {
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
                });
                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id= "Bearer"
                            },
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },new List<string>()
                    }
                });
            });

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();
           
            app.MapControllers();

            app.MapPost("add-product", async (ProductDto Dto, IUnitOfWork context) =>
            {
                await context.Products.Add(new()
                {
                    Description = Dto.Description,
                    Name = Dto.Name,
                    Image = "path",
                    CategoryId = Dto.CategoryId,
                    price = Dto.price,
                    Quantity = Dto.Quantity,
                });
                return Results.Ok();
            }).Accepts<IFormFile>("multipart/form-data");

          

            app.Run();
        }
    }

    class Product_Dto
    {
		[Required]
		public string Name { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public double price { get; set; }
		[Required]
		public int CategoryId { get; set; }
	}
}