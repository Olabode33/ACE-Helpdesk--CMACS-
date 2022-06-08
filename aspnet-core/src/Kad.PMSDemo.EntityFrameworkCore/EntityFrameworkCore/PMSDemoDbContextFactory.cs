using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Kad.PMSDemo.Configuration;
using Kad.PMSDemo.Web;

namespace Kad.PMSDemo.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class PMSDemoDbContextFactory : IDesignTimeDbContextFactory<PMSDemoDbContext>
    {
        public PMSDemoDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PMSDemoDbContext>();
            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true
            );

            PMSDemoDbContextConfigurer.Configure(builder, configuration.GetConnectionString(PMSDemoConsts.ConnectionStringName));

            return new PMSDemoDbContext(builder.Options);
        }
    }
}