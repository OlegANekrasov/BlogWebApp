namespace BlogWebApp.DAL.Interfaces
{
    /// <summary>
    /// Interface UoW
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Saving all changes to the database (for all repositories)
        /// </summary>
        /// <param name="ensureAutoHistory"></param>
        /// <returns></returns>
        int SaveChanges(bool ensureAutoHistory = false);

        /// <summary>
        /// Returns user repositories
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="hasCustomRepository"></param>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class;
    }
}
