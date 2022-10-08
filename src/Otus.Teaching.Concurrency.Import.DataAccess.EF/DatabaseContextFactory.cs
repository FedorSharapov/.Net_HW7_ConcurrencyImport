using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.Concurrency.Import.DataAccess.EF
{
    public static class DatabaseContextFactory
    {
        public static  DatabaseContext CreateDbContext(string sqlProvider, string connectionString)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            if (sqlProvider.Equals(TypeDataBase.PostgreSQL))
                dbContextOptionsBuilder.UseNpgsql(connectionString);
            else if (sqlProvider.Equals(TypeDataBase.SQLite))
                dbContextOptionsBuilder.UseSqlite(connectionString);
            else
                throw new System.Exception($"Invalid type database \"{sqlProvider}\".");
            
            return new DatabaseContext(dbContextOptionsBuilder.Options);
        }
    }
}