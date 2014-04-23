using NUnit.Framework;
using StructureMap;
using StructureMap.Configuration.DSL;
using Test_InvoiceService;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure.Implemented;
using Trex.ServiceContracts;
using TrexSL.Web;

namespace Test_CustomerInvoiceGroup
{
    //[TestFixture]
    //public class GetCustomerInvoiceGroupByCustomerId
    //{
    //    #region Setup/Teardown

    //    [SetUp]
    //    public void Setup()
    //    {
    //        ObjectFactory.Initialize(fac =>
    //            {
    //                fac.UseDefaultStructureMapConfigFile = false;
    //                fac.AddRegistry<TestStructureMapRegistry>();
    //            }
    //        );
    //    }

    //    [TearDown]
    //    public void TearDown()
    //    {
    //    }

    //    #endregion

    //    [Test]
    //    public void GetCustomerInvoiceGroupByCustomerId_RealData_ReturnsOneRows()
    //    {
    //        int customerId = 5000;

    //        var trexSLService = new TrexSLService();

    //        Assert.DoesNotThrow(() => trexSLService.GetCustomerInvoiceGroupByCustomerId(customerId));
    //    }
    //}

}
