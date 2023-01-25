﻿using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlogWebApp.DAL.Repository
{
    /// <summary>
    /// Implements the UoW pattern
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _appContext;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(ApplicationDbContext app)
        {
            _appContext = app;
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// Returns user repositories
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="hasCustomRepository"></param>
        /// <returns></returns>
        public IRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = true) where TEntity : class
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            if (hasCustomRepository)
            {
                var customRepo = _appContext.GetService<IRepository<TEntity>>();
                if (customRepo != null)
                {
                    return customRepo;
                }
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<TEntity>(_appContext);
            }

            return (IRepository<TEntity>)_repositories[type];

        }

        public int SaveChanges(bool ensureAutoHistory = false)
        {
            throw new NotImplementedException();
        }
    }
}
