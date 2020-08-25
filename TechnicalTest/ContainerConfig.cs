using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gluh.TechnicalTest
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<PurchaseOptimizer>().As<IPurchaseOptimizer>();
            builder.RegisterType<TestData>().As<ITestData>();

            return builder.Build();
        }
    }
}
