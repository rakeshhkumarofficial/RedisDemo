using NHibernate;
using NHibernate.Cfg;
using RedisDemo.Models;
using ISession = NHibernate.ISession;

namespace RedisDemo.SessionFactory
{
    public class NHibernateSessionFactory : INHibernateSessionFactory
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateSessionFactory()
        {
            var configuration = new Configuration();
            configuration.Configure(Path.Combine(Directory.GetCurrentDirectory(),"Data/hibernate.cfg.xml"));
            configuration.AddAssembly(typeof(Product).Assembly);
            _sessionFactory = configuration.BuildSessionFactory();
        }

        public ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}
