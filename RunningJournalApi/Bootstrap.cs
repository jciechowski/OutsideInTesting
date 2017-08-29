using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Newtonsoft.Json.Serialization;
using RunningJournalApi.Properties;

namespace RunningJournalApi
{
    public class Bootstrap
    {
        public void Configure(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                "API Default",
                "{controller}/{id}",
                new
                {
                    controller = "Journal",
                    id = RouteParameter.Optional
                });
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            config.Services.Replace(typeof(IHttpControllerActivator), new CompositionRoot());
        }

        public static void InstallDatabase()
        {
            var connStr =
                ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            InstallDatabase(connStr);
        }

        private static void InstallDatabase(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString) {InitialCatalog = "Master"};
            using (var conn = new SqlConnection(builder.ConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    var schemaSql = Resources.RunningDbSchema;
                    foreach (var sql in
                        schemaSql.Split(
                            new[] {"GO"},
                            StringSplitOptions.RemoveEmptyEntries))
                    {
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void UninstallDatabase()
        {
            var connStr =
                ConfigurationManager.ConnectionStrings["running-journal"].ConnectionString;
            UninstallDatabase(connStr);
        }

        private static void UninstallDatabase(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString) {InitialCatalog = "Master"};
            using (var conn = new SqlConnection(builder.ConnectionString))
            {
                conn.Open();

                const string dropCmd = @"
                    IF EXISTS (SELECT name
                               FROM master.dbo.sysdatabases
                               WHERE name = N'RunningJournal')
                    DROP DATABASE [RunningJournal];";
                using (var cmd = new SqlCommand(dropCmd, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}