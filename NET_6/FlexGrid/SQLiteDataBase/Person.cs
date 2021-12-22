using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace SQLiteDataBase
{
    public class PeopleContext : DbContext
    {
        public DbSet<Person> Person { get; set; }
        public static readonly LoggerFactory _myLoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SQLiteDataBase.db3");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            //optionsBuilder.UseLoggerFactory(_myLoggerFactory);
        }
    }

    public class Person
    {
        public Person() { }

        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: ID={0}, FirstName={1}, LastName={2}]", ID, FirstName, LastName);
        }
    }
}
