using Gluh.TechnicalTest.Database;

namespace Gluh.TechnicalTest
{
    public interface IPurchaseOptimizer
    {
        decimal CalculateTotalCostWithShipping(decimal cost, int quantity, Supplier supplier);
        void Optimize();
    }
}