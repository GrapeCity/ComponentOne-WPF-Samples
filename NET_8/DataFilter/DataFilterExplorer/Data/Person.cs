using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace DataFilterExplorer
{
    public class PeopleContext : DbContext
    {
        public DbSet<Person> Person { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DataFilterVirtualSource.db3");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }

    public class Person
    {
        public Person() { }

        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public override string ToString()
        {
            return string.Format("[Person: ID={0}, FirstName={1}, LastName={2}]", ID, FirstName, LastName);
        }
    }
}
