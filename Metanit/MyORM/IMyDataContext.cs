namespace MyORM;

public interface IMyDataContext
{
    bool Add<T>(T entity);
    bool Update<T>(T entity);
    bool Delete<T>(T entity);
    List<T> Select<T>();
    T SelectById<T>(int id);
}