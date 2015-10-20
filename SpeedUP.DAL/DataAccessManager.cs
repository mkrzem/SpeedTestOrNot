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

        public static void InitDataService(string serviceType)
        {
            #region NHibernate
            var configuration = new Configuration();
            configuration.Configure();
            configuration.AddAssembly(typeof(Car).Assembly);

            new SchemaExport(configuration).Execute(false, true, false);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            session = sessionFactory.OpenSession();
            #endregion

            #region MongoDB
            InitMongoAsync();

            //cars.InsertOneAsync(newCar);

            #endregion
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

        public static async Task<string> SaveCarsAsync(int carCount)
        {
            session.FlushMode = FlushMode.Commit;
            Stopwatch watch = new Stopwatch();
            //watch.Start();
            await Task.Run(() =>
            {
                using (var transaction = session.BeginTransaction())
                {
                    watch.Start();
                    var random = new Random();
                    List<Manufacturer> manus = GetManufacturers();
                    for (int i = 0; i < carCount; i++)
                    {
                        int manufacturerNumber = random.Next(10);
                        int PartsCount = random.Next(50);
                        List<Part> parts = new List<Part>();
                        

                        var carEntity = new Car()
                        {
                            Make = "StillVolkswagen",
                            Model = "AlwaysBora",
                            Year = i / 1000,

                        };
                        
                        for (int j = 0; j < PartsCount; j++)
                        {
                            parts.Add(new Part { Car = carEntity, Manufacturer = manus[manufacturerNumber], PartName = string.Format("PartType{0}", j)});
                        }

                        session.Save(carEntity);
                    }
                    watch.Stop();
                    transaction.Commit();                    
                }
            });
            //watch.Stop();

            session.FlushMode = FlushMode.Auto;

            return watch.ElapsedMilliseconds.ToString();
        }

        private static List<Manufacturer> GetManufacturers()
        {
            List<Manufacturer> manu = new List<Manufacturer>();
            manu.AddRange(new Manufacturer[]
            {
                            new Manufacturer { Name = "ManA" }, new Manufacturer { Name = "ManB" },
                            new Manufacturer { Name = "ManC" }, new Manufacturer { Name = "ManD" },
                            new Manufacturer { Name = "ManE" }, new Manufacturer { Name = "ManF" },
                            new Manufacturer { Name = "ManG" }, new Manufacturer { Name = "ManH" },
                            new Manufacturer { Name = "ManI" }, new Manufacturer { Name = "ManJ" },
            });

            return manu;
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
