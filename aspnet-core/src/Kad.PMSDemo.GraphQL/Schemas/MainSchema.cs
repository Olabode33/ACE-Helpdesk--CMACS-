using Abp.Dependency;
using GraphQL;
using GraphQL.Types;
using Kad.PMSDemo.Queries.Container;

namespace Kad.PMSDemo.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IDependencyResolver resolver) :
            base(resolver)
        {
            Query = resolver.Resolve<QueryContainer>();
        }
    }
}