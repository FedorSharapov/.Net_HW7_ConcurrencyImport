using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.Concurrency.Import.DataAccess.EF
{
    public static class DatabaseContextFactory
    {
        public static  DatabaseContext CreateDbContext(string sqlProvider, string connectionString)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            if (sqlProvider.Equals(TypesDatabases.PostgreSQL))
                dbContextOptionsBuilder.UseNpgsql(connectionString);
            else if (sqlProvider.Equals(TypesDatabases.SQLite))
                dbContextOptionsBuilder.UseSqlite(connectionString);
            else
                throw new System.Exception($"Invalid type database \"{sqlProvider}\".");
            
            return new DatabaseContext(dbContextOptionsBuilder.Options);
        }
    }
}