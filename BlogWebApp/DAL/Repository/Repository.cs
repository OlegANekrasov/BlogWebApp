using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbContext _db;

        public DbSet<T> Set { get; private set; }

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            var set = _db.Set<T>();
            set.Load();

            Set = set;
        }

        /// <summary>
        /// Adding an entry to the database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateAsync(T item)
        {
            Set.Add(item);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Deleting an entry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task DeleteAsync(T item)
        {
            Set.Remove(item);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Selecting a record by ID from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string id)
        {
            return await Set.FindAsync(id);
        }

        /// <summary>
        /// Selecting all records from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            return Set;
        }

        /// <summary>
        /// Editing existing entries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T item)
        {
            Set.Update(item);
            await _db.SaveChangesAsync();
        }
    }
}
