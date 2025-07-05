﻿using System.Linq.Expressions;

namespace FUMini.DataAccess.Repositories.Base
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieves a list of entities from the database with optional filtering, ordering, and eager loading.
        /// </summary>
        /// <param name="filter">
        /// Optional filter expression (e.g., x => x.IsActive).
        /// </param>
        /// <param name="orderBy">
        /// Optional ordering function (e.g., q => q.OrderBy(x => x.Name)).
        /// </param>
        /// <param name="includeProperties">
        /// Comma-separated list of navigation properties to include (e.g., "Category,Tags").
        /// </param>
        /// <returns> 
        ///  IEnumerable of filtered, ordered, and optionally eager-loaded entities.
        /// </returns>
        public IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        public TEntity GetByID(object id);
        public void Insert(TEntity entity);

        /// <summary>
        /// Deletes an entity from the database by its primary key. (MUST CALL SAVE MANUALLY)
        /// </summary>
        /// <param name="id">The primary key of the entity to delete.</param>
        public void Delete(object id);

        /// <summary>
        /// Deletes an entity instance from the database.(MUST CALL SAVE MANUALLY) 
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        public void Delete(TEntity entityToDelete);

        /// <summary>
        /// Updates an existing entity in the database context. (MUST CALL SAVE MANUALLY)
        /// </summary>
        /// <param name="entityToUpdate">The entity with updated values.</param>
        public void Update(TEntity entityToUpdate);

        /// <summary>
        /// Commits all pending changes in the database context.
        /// </summary>
        public void Save();
    }
}
