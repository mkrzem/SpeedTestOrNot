using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SpeedUp.Domain;

namespace SpeedUP.DAL
{
    public static class DataAccessManager
    {
        public static void GetDataService(string serviceType)
        {
            var cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(typeof(Car).Assembly);

            new SchemaExport(cfg).Execute(false, true, false);


        }
    }
}
