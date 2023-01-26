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
using static System.Net.Mime.MediaTypeNames;
using NLog;
using NLog.Web;
using Microsoft.CodeAnalysis.Differencing;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.Hubs;
using BlogWebApp.BLL;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddMvc();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

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
        .AddRoles<ApplicationRole>()   // долно быть первым
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

    builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
    builder.Services.AddSession();

    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    builder.Services.AddSignalR();

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
            // Резервная политика авторизации:
            // Применяется ко всем запросам, которые явно не указывают политику авторизации.
            // Для запросов, обслуживаемых маршрутизацией конечных точек, сюда входит любая конечная точка, не указывающая атрибут авторизации.
            // Для запросов, обслуживаемых другим ПО промежуточного слоя после ПО промежуточного слоя авторизации,
            // например статических файлов, эта политика применяется ко всем запросам.
            .RequireAuthenticatedUser()
            .Build();

        // Для проверки ролей на основе политик
        options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Администратор"));
        options.AddPolicy("RequireModeratorRole", policy => policy.RequireRole("Модератор"));
    });

    builder.Services.AddUnitOfWork();
    builder.Services.AddCustomRepository<BlogArticle, BlogArticlesRepository>();
    builder.Services.AddCustomRepository<Tag, TagsRepository>();
    builder.Services.AddCustomRepository<Comment, CommentsRepository>();

    builder.Services.AddScoped<IBlogArticleService, BlogArticleService>();
    builder.Services.AddScoped<ITagService, TagService>();
    builder.Services.AddScoped<ICommentService, CommentService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IRoleService, RoleService>();

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
            logger.Error(ex, "An error occurred while seeding the database.");
        }
    }

    app.Environment.EnvironmentName = "Production";

    // Configure the HTTP request pipeline. 
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
        app.UseDeveloperExceptionPage();
}
    else
    {
        app.UseExceptionHandler("/Home/SomethingWentWrong");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.Use(async (context, next) =>
    {
        await next();
        if (context.Response.StatusCode == 404)
        {
            context.Request.Path = "/NotFound";
            await next();
        }
    });

    app.UseHttpsRedirection();

    var cachePeriod = "0";
    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx =>
        {
            ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}");
        }
    });

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
        endpoints.MapHub<BlogHub>("/blogHub");
    });
    
    app.Run();   
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}