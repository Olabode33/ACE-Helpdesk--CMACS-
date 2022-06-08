using Kad.PMSDemo.Auditing;
using Kad.PMSDemo.Test.Base;
using Shouldly;
using Xunit;

namespace Kad.PMSDemo.Tests.Auditing
{
    // ReSharper disable once InconsistentNaming
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("Kad.PMSDemo.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("Kad.PMSDemo.Auditing.GenericEntityService`1[[Kad.PMSDemo.Storage.BinaryObject, Kad.PMSDemo.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("Kad.PMSDemo.Auditing.XEntityService`1[Kad.PMSDemo.Auditing.AService`5[[Kad.PMSDemo.Storage.BinaryObject, Kad.PMSDemo.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[Kad.PMSDemo.Storage.TestObject, Kad.PMSDemo.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
