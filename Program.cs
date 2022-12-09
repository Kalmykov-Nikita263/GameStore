using GameStore.Domain.Repository.Abstractions;
using GameStore.Domain.Repository.EntityFramework;
using GameStore.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GameStore.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace GameStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Сопоставляем Project из appsettings.json с классом Configuration
            builder.Configuration.Bind("Project", new Configuration());

            //Подключаем доменную модель в качестве сервисов
            builder.Services.AddTransient<IGamesRepository, EFGamesRepository>();
            builder.Services.AddTransient<DataManager>();

            //Подключаем контекст базы данных в качестве сервиса
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.ConnectionString, output => output.MigrationsAssembly("GameStore"));
            });

            //Настраиваем Identity систему
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(Settings =>
            {
                Settings.User.RequireUniqueEmail = true;
                Settings.Password.RequiredLength = 6;
                Settings.Password.RequireNonAlphanumeric = false;
                Settings.Password.RequireUppercase = false;
                Settings.Password.RequireLowercase = false;
                Settings.Password.RequireDigit = false;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            //Настраиваем куки приложения
            builder.Services.ConfigureApplicationCookie(CookieOptions =>
            {
                CookieOptions.Cookie.Name = "GameStoreAuth";
                CookieOptions.Cookie.HttpOnly = true;
                CookieOptions.LoginPath = "/account/login";
                CookieOptions.AccessDeniedPath = "/account/accessdenied";
                CookieOptions.SlidingExpiration = true;
            });

            //Настраиваем политику авторизации для Admin area
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminArea", policy => policy.RequireRole("admin"));
            });

            //Подключаем представления с контроллерами. Добавляем соглашение с авторизацией админа
            builder.Services.AddControllersWithViews(options =>
            {
                options.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            //Подключаем перенаправление по протоколу Https
            app.UseHttpsRedirection();

            //Подключаем поддержку статических файлов приложения (картинки, стили, js)
            app.UseStaticFiles();

            //Подключаем систему маршрутизации
            app.UseRouting();

            //Подключаем cookie, авторизацию, аутентификаци.
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //Сопаставляем маршруты по заданному шаблону
            app.MapControllerRoute(
                name: "admin",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            //Запускаем приложение
            app.Run();
        }
    }
}