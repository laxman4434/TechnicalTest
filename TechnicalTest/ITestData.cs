using Gluh.TechnicalTest.Models;
using System.Collections.Generic;

namespace Gluh.TechnicalTest
{
    public interface ITestData
    {
        List<PurchaseRequirement> Create();
    }
}