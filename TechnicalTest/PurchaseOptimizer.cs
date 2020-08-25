using System;
using System.Collections.Generic;
using System.Text;
using Gluh.TechnicalTest.Models;
using Gluh.TechnicalTest.Database;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Gluh.TechnicalTest
{
    public class PurchaseOptimizer : IPurchaseOptimizer
    {
        private readonly ITestData _itestData;

        PurchaseOrder purchaseOrder = null;
        readonly List<PurchaseOrder> totalPurchaseOrders = new List<PurchaseOrder>();

        public PurchaseOptimizer(ITestData itestData)
        {
            _itestData = itestData;
        }

        /// <summary>
        /// Calculates the optimal set of supplier to purchase products from.
        /// ### Complete this method
        /// </summary>
        /// Assumptions
        /// Can't make a purchase oredr more than once from the supplier
        /// Return optimal list of all suppliers for the product irresespective of availability
        public void Optimize()
        {
            List<PurchaseRequirement> purchaseRequirements = _itestData.Create();

            purchaseRequirements.ForEach(pr =>
            {
                // Print the Product Name, Quantity Required & Product Type
                Console.WriteLine($"Product : {pr.Product.Name} \nQuantity Required : {pr.Quantity} \nProduct Type : {pr.Product.Type} \n");

                //Get the list of product & suppllier details with availability of stock
                List<ProductStock> availableProductStock = pr.Product.Stock.Where(c => c.StockOnHand > 0).ToList();
                //Get the list of product & suppllier details where stock is unavailable
                List<ProductStock> unavailableProductStock = pr.Product.Stock.Where(c => c.StockOnHand <= 0).ToList();

                Console.WriteLine($"Total Available Suppliers : {availableProductStock.Count}   Unavailable Suppliers : {unavailableProductStock.Count} ");
                Console.WriteLine("------------------------------------------------------\n");

                PrintData(pr, pr.Product.Stock.ToList());

                Console.WriteLine("*******************************************************\n");
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseRequirement"></param>
        /// <param name="productStocks"></param>
        private void PrintData(PurchaseRequirement purchaseRequirement, List<ProductStock> productStocks)
        {
            int remainingQuantity = purchaseRequirement.Quantity;
            int orderQuantity = 0;
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            productStocks = productStocks.Where(c => c.StockOnHand > 0).OrderBy(d => d.Cost).ToList();

            if (productStocks.Count == 0)
            {
                Console.WriteLine("Order cannot be placed due to unavailability.\n");
                return;
            }

            foreach (ProductStock productStock in productStocks)
            {
                decimal totalCostWithShipping = 0m;

                if (remainingQuantity > 0)
                {
                    orderQuantity = remainingQuantity - productStock.StockOnHand;

                    if (orderQuantity > 0)
                        orderQuantity = productStock.StockOnHand;

                    if (orderQuantity < 0)
                        orderQuantity += productStock.StockOnHand;

                    purchaseOrder = new PurchaseOrder
                    {
                        QuantityOrdered = orderQuantity,
                        SupplierName = productStock.Supplier.Name,
                        ProductName = productStock.Product.Name,
                        //SupplierID = productStock.Supplier.ID
                    };

                    //check if purchase order has been placed already from the suppplier
                    if (!totalPurchaseOrders.Any(q => q.SupplierName == purchaseOrder.SupplierName))
                    {
                        totalCostWithShipping = CalculateTotalCostWithShipping(productStock.Cost, orderQuantity, productStock.Supplier);
                        purchaseOrder.TotalCost = totalCostWithShipping;

                        purchaseOrders.Add(purchaseOrder);
                        totalPurchaseOrders.Add(purchaseOrder);

                        remainingQuantity -= orderQuantity;
                    }
                    else { orderQuantity = 0; }
                }
                else
                { orderQuantity = 0; }

                PrintProductDetails(productStock, orderQuantity);
            }

            PrintPurchaseOrderDetails(purchaseOrders);
        }

        /// <summary>
        /// Print all the supplier details, product details and shipping info on to the console
        /// </summary>
        /// <param name="productStock"></param>
        /// <param name="orderQuantity"></param>
        private void PrintProductDetails(ProductStock productStock, int orderQuantity)
        {
            Console.WriteLine($"Product Cost : {productStock.Cost} \nStockOnHand : {productStock.StockOnHand}");
            Console.WriteLine($"Supplier Name : {productStock.Supplier.Name}");
            Console.WriteLine($"Quantity Ordered : {orderQuantity}");
            if (orderQuantity > 0)
                Console.WriteLine($"Total cost including shipping: {CalculateTotalCostWithShipping(productStock.Cost, orderQuantity, productStock.Supplier)} \n");
            else
                Console.WriteLine();
        }

        /// <summary>
        /// Print the purchase Order details on to the console
        /// </summary>
        /// <param name="purchaseOrders"></param>
        private void PrintPurchaseOrderDetails(List<PurchaseOrder> purchaseOrders)
        {
            purchaseOrders.OrderBy(m => m.QuantityOrdered).ToList().ForEach(s =>
            {
                Console.WriteLine($"Order placed with {s.SupplierName} for {s.QuantityOrdered} {s.ProductName} with totalcost including shipping {s.TotalCost}\n");
            });

            if (!purchaseOrders.Any())
                Console.WriteLine($"Order could not be placed eventhough suppliers are available, because a maximum of one purchase order can be placed from a supplier\n");
        }


        /// <summary>
        /// Calculate the total cost of the purchase order including shipping considering the minimum order value & maximum order value
        /// </summary>
        /// <param name="cost"></param>
        /// <param name="quantity"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public decimal CalculateTotalCostWithShipping(decimal cost, int quantity, Supplier supplier)
        {
            var totalcost = cost * quantity;
            var shipping = 0m;

            if (totalcost < supplier.ShippingCostMinOrderValue || totalcost > supplier.ShippingCostMaxOrderValue)
                shipping += supplier.ShippingCost;

            totalcost += shipping;

            return totalcost;
        }

    }
}
