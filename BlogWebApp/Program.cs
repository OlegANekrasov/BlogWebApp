using AutoMapper;
using BlogWebApp;
using BlogWebApp.BLL.Services;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Extentions;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();

var mapperConfig = new MapperConfiguration((v) =>
{
    v.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var connectionString = builder.Configuration.GetConnectionString("BlogWebAppContextConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()   // долно быть первым
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
builder.Services.AddSession();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        // –езервна€ политика авторизации:
        // ѕримен€етс€ ко всем запросам, которые €вно не указывают политику авторизации.
        // ƒл€ запросов, обслуживаемых маршрутизацией конечных точек, сюда входит люба€ конечна€ точка, не указывающа€ атрибут авторизации.
        // ƒл€ запросов, обслуживаемых другим ѕќ промежуточного сло€ после ѕќ промежуточного сло€ авторизации,
        // например статических файлов, эта политика примен€етс€ ко всем запросам.
        .RequireAuthenticatedUser()
        .Build();
    
    // ƒл€ проверки ролей на основе политик
    options.AddPolicy("RequireAdministratorRole",
         policy => policy.RequireRole("администратор"));
});

builder.Services.AddUnitOfWork();
builder.Services.AddCustomRepository<BlogArticle, BlogArticlesRepository>();
builder.Services.AddCustomRepository<Tag, TagsRepository>();
builder.Services.AddCustomRepository<Comment, CommentsRepository>();

builder.Services.AddScoped<IBlogArticleService, BlogArticleService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await AdminInitializer.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
