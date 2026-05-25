using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SQLiteDataBase.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

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

    /// <summary>
    /// Represents a person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// The id number.
        /// </summary>
        [Display(Name = nameof(AppResources.IdLabel), ResourceType = typeof(AppResources))]
        public int ID { get; set; }

        /// <summary>
        /// The first name of the person.
        /// </summary>
        [Display(Name = nameof(AppResources.FirstNameLabel), ResourceType = typeof(AppResources))]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the person.
        /// </summary>
        [Display(Name = nameof(AppResources.LastNameLabel), ResourceType = typeof(AppResources))]
        public string LastName { get; set; }

        ///<inheritdoc/>
        public override string ToString()
        {
            return string.Format("[Person: ID={0}, FirstName={1}, LastName={2}]", ID, FirstName, LastName);
        }
    }
}
