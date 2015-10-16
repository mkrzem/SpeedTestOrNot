using System;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using System.Collections.Generic;
using SpeedUP.DAL.Domain;
using System.Threading.Tasks;

namespace SpeedUP.DAL
{
    public static class DataAccessManager
    {
        private static ISession session;
        public static void InitDataService(string serviceType)
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

        public static async Task<IList<Car>> ReadCarsAsync()
        {
            IList<Car> result = new List<Car>();
            if (session?.IsConnected == true && session?.IsOpen == true)
            {
                await Task.Run(() =>
                {
                    IQueryOver<Car> query = session.QueryOver<Car>();
                    result = query.List<Car>();
                });
            }

            return result;
        }

        public static async Task SaveCarsAsync(int carCount)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < carCount; i++)
                {
                    var carEntity = new Car()
                    {
                        Make = "StillVolkswagen",
                        Model = "AlwaysBora",
                        Year = i / 1000
                    };
                    session.Save(carEntity);
                }
                session.Flush();
            });
        }

        public static void SaveCars(int carCount)
        {
            for (int i = 0; i < carCount; i++)
            {
                var carEntity = new Car()
                {
                    Make = "StillVolkswagen",
                    Model = "AlwaysBora",
                    Year = i / 1000
                };
                session.Save(carEntity);
            }
            session.Flush();
        }

        public static void ClearCars()
        {
            if (session?.IsConnected == true && session?.IsOpen == true)
            {
                session.Delete("from Car");
                session.Flush();
            }
        }
    }
}
