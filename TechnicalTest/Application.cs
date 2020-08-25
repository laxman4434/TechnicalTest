using System;
using System.Collections.Generic;
using System.Text;

namespace Gluh.TechnicalTest
{
    public class Application : IApplication
    {
        private readonly IPurchaseOptimizer _purchaseOptimizer;

        public Application(IPurchaseOptimizer purchaseOptimizer)
        {
            _purchaseOptimizer = purchaseOptimizer;
        }

        public void Run()
        {
            _purchaseOptimizer.Optimize();
        }
    }
}
