namespace KinoSearch.Ratings.Data
{
    public interface IRepository<TEntity>
    {
        void Create(TEntity entity);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetWhere(Func<TEntity, bool> predicate);
        bool Delete(TEntity entity);
    }

    public interface IRepository<TId, TEntity> : IRepository<TEntity>
    {
        TEntity? GetById(TId id);
    }
}
