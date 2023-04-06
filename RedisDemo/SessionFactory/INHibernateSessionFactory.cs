using ISession = NHibernate.ISession;
namespace RedisDemo.SessionFactory
{
    public interface INHibernateSessionFactory
    {
        ISession OpenSession();
    }
}
