using System;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using System.Collections.Generic;
using SpeedUP.DAL.Domain;

namespace SpeedUP.DAL
{
    public static class DataAccessManager
    {
        private static ISession session;
        public static void GetDataService(string serviceType)
        {
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Car).Assembly);

            //new SchemaExport(configuration).Execute(false, true, false);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            session = sessionFactory.OpenSession();

            //var car = new Car()
            //{
            //    Make = "Volkswagen",
            //    Model = "Bora",
            //    Year = 2000
            //};

            //session.Save(car);
            //session.Flush();
        }

        public static IList<Car> ReadCars()
        {
            IList<Car> result = new List<Car>();
            if (session?.IsConnected == true && session?.IsOpen == true)
            {
                IQueryOver<Car> query = session.QueryOver<Car>();
                result = query.List<Car>();
            }

            return result;
        }
    }
}
