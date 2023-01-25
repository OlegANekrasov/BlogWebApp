using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Repository;

namespace BlogWebApp.DAL.Extentions
{
    /// <summary>
    /// IServiceCollection extension methods
    /// </summary>
    public static class ServiceExtentions
    {
        /// <summary>
        /// Adding UnitOfWork service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Adding Custom Repositories
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="IRepository"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomRepository<TEntity, IRepository>(this IServiceCollection services)
                 where TEntity : class
                 where IRepository : class, IRepository<TEntity>
        {
            services.AddScoped<IRepository<TEntity>, IRepository>();

            return services;
        }
    }
}
