using Gluh.TechnicalTest.Database;
using Moq;
using NUnit.Framework;

namespace Gluh.TechnicalTest.UnitTests
{
    [TestFixture]
    public class Tests
    {
        private IPurchaseOptimizer _purchaseOptimizer;
        private Mock<ITestData> _itestData;

        [SetUp]
        public void Setup()
        {
            _itestData = new Mock<ITestData>();
            _purchaseOptimizer = new PurchaseOptimizer(_itestData.Object);

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
        [TestCase(120, 10, 1210)]
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