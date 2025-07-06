namespace FUMini.Services.Base
{
    public interface IBaseService<T, C>
    {
        List<T> GetAll();
        T GetByID(C id);
        void Add(T entity);
        void Update(T entity);
        void Delete(C id);
        void Delete(T entity);
    }
}
