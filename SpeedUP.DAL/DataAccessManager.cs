using MongoDB.Driver;
using MongoDB.Bson;
using NHibernate.Cfg;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using SpeedUP.DAL.Domain;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using NHibernate.Tool.hbm2ddl;

namespace SpeedUP.DAL
{
    public static class DataAccessManager
    {
        private static ISession session;
        private static IMongoClient client;
        private static IMongoDatabase database;
        private static ISessionFactory sessionFactory;

        public static void InitDataService(string serviceType)
        {
            #region NHibernate
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Car).Assembly);

            //new SchemaExport(configuration).Execute(false, true, false);

            sessionFactory = configuration.BuildSessionFactory();            
            session = sessionFactory.OpenSession();
            //CreateManufacturers();
            #endregion

            #region MongoDB
            InitMongoAsync();

            //cars.InsertOneAsync(newCar);

            #endregion
        }

        private static void CreateManufacturers()
        {
            using (var transaction = session.BeginTransaction())
            {
                session.Save(new Manufacturer { Name = "ManA" });
                session.Save(new Manufacturer { Name = "ManB" });
                session.Save(new Manufacturer { Name = "ManC" });
                session.Save(new Manufacturer { Name = "ManD" });
                session.Save(new Manufacturer { Name = "ManE" });
                session.Save(new Manufacturer { Name = "ManF" });
                session.Save(new Manufacturer { Name = "ManG" });
                session.Save(new Manufacturer { Name = "ManH" });
                session.Save(new Manufacturer { Name = "ManI" });
                session.Save(new Manufacturer { Name = "ManJ" });

                transaction.Commit();
            }
        }

        private static void InitMongoAsync()
        {
            client = new MongoClient();
            database = client.GetDatabase("mongoDB");
            //var newCar = new BsonDocument
            //{
            //    { "Make", "StillVolkswagen" },
            //    { "Model", "AlwaysBora" },
            //    { "Year", new BsonInt32(2000) }
            //};

            //IMongoCollection<BsonDocument> cars = database.GetCollection<BsonDocument>("Car");
            //using (var cursor = await cars.FindAsync(new BsonDocument()))
            //{
            //    await cursor.MoveNextAsync();
            //    foreach (BsonDocument document in cursor.Current)
            //    {
            //        document.Elements.Count();
            //    }
            //}
        }

        public static async Task<IList<Car>> ReadCarsAsync(string hibernateQuery)
        {
            if (string.IsNullOrEmpty(hibernateQuery))
            {
                hibernateQuery = "from Car";
            }

            IList<Car> result = new List<Car>();
            if (session?.IsConnected == true && session?.IsOpen == true)
            {
                await Task.Run(() =>
                {
                    result = session.CreateQuery(hibernateQuery).List<Car>();
                    //IQueryOver<Car> query = session.QueryOver<Car>();
                    //query.
                    //result = query.List<Car>();
                });
            }

            return result;
        }

        public static async Task<string> CreateAndSaveCarsAsync(int carsCount, IProgress<int> progress = null)
        {
            
            Stopwatch watch = new Stopwatch();
            //watch.Start();
            await Task.Run(() =>
            {
                using(var saveSession = sessionFactory.OpenStatelessSession())
                using (var transaction = saveSession.BeginTransaction())
                {
                    watch.Start();
                    var random = new Random();
                    IList<Manufacturer> manufacturers = GetManufacturers();
                    for (int current = 0; current < carsCount; current++)
                    {
                        Car carEntity = CreateNewCar(random, manufacturers, current);

                        saveSession.Insert(carEntity);

                        progress?.Report((int)(((float)current / carsCount) * 100));
                    }

                    //watch.Start();
                    transaction.Commit();
                    watch.Stop();
                }
            });
            //watch.Stop();

            //session.FlushMode = FlushMode.Auto;

            return watch.ElapsedMilliseconds.ToString();
        }

        private static Car CreateNewCar(Random random, IList<Manufacturer> manus, int current)
        {
            int manufacturerNumber = random.Next(10);
            int PartsCount = random.Next(50);

            var carEntity = new Car()
            {
                Make = "StillVolkswagen",
                Model = "AlwaysBora",
                Year = current / 1000,
                Parts = new List<Part>()
            };

            for (int j = 0; j < PartsCount; j++)
            {
                carEntity.Parts.Add(new Part { Car = carEntity, Manufacturer = manus[manufacturerNumber], PartName = string.Format("PartType{0}", j) });
            }

            return carEntity;
        }

        private static IList<Manufacturer> GetManufacturers()
        {
            return session.QueryOver<Manufacturer>().List();
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

        public static void ChangeDatabase(string database)
        {
            
        }
    }
}
