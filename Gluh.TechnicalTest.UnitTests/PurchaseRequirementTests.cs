using Gluh.TechnicalTest.Database;
using NUnit.Framework;

namespace Gluh.TechnicalTest.UnitTests
{
    [TestFixture]
    public class Tests
    {
        private PurchaseOptimizer _purchaseOptimizer;

        [SetUp]
        public void Setup()
        {
            _purchaseOptimizer = new PurchaseOptimizer();
        }

        [Test]
        [TestCase(16, 10, 160)]
        [TestCase(90, 10, 900)]
        public void CalculateTotalCostWithShipping_WhenPurchasedBetweenOrderValues_ReturnTotalCostWithZeroShipping(int productCost, int Quantity, int expectedResult)
        {
            var result = _purchaseOptimizer.CalculateTotalCostWithShipping(productCost, Quantity, new Supplier
            {
                ShippingCost = 10,
                ShippingCostMaxOrderValue = 1000,
                ShippingCostMinOrderValue = 150
            });

            Assert.AreEqual(result, expectedResult);
        }

        [Test]
        [TestCase(13, 10, 140)]
        [TestCase(120, 10, 900)]
        public void CalculateTotalCostWithShipping_WhenPurchasedBelowAndAboveOrderValues_ReturnTotalCostWithShipping(int productCost, int Quantity, int expectedResult)
        {
            var result = _purchaseOptimizer.CalculateTotalCostWithShipping(productCost, Quantity, new Supplier
            {
                ShippingCost = 10,
                ShippingCostMaxOrderValue = 1000,
                ShippingCostMinOrderValue = 150
            });

            Assert.AreEqual(result, expectedResult);
        }
    }
}