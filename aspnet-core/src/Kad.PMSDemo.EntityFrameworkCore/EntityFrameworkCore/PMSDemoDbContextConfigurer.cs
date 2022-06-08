using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Kad.PMSDemo.EntityFrameworkCore
{
    public static class PMSDemoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<PMSDemoDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<PMSDemoDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}