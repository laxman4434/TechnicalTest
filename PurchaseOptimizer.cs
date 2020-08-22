using System;
using System.Collections.Generic;
using System.Text;
using Gluh.TechnicalTest.Models;
using Gluh.TechnicalTest.Database;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Gluh.TechnicalTest
{
    public class PurchaseOptimizer
    {
        PurchaseOrder purchaseOrder = null;

        /// <summary>
        /// Calculates the optimal set of supplier to purchase products from.
        /// ### Complete this method
        /// </summary>
        public void Optimize(List<PurchaseRequirement> purchaseRequirements)
        {

            foreach (PurchaseRequirement purchaseRequirement in purchaseRequirements)
            {
                // Print the Product Name, Quantity Required & Product Type
                Console.WriteLine($"Product : {purchaseRequirement.Product.Name} \nQuantity Required : {purchaseRequirement.Quantity} \nProduct Type : {purchaseRequirement.Product.Type} \n");

                //Get the list of product & suppllier details with availability of stock
                List<ProductStock> availableProductStock = purchaseRequirement.Product.Stock.Where(c => c.StockOnHand >= purchaseRequirement.Quantity).ToList();
                //Get the list of product & suppllier details where stock is unavailable
                List<ProductStock> unavailableProductStock = purchaseRequirement.Product.Stock.Where(c => c.StockOnHand < purchaseRequirement.Quantity).ToList();

                Console.WriteLine($"Total Available Suppliers : {availableProductStock.Count}   Unavailable Suppliers : {unavailableProductStock.Count} ");
                Console.WriteLine("------------------------------------------------------\n");

                //Print the supplier & product details, purchase requirement details on to the console
                PrintAvilableSuppliers(purchaseRequirement, availableProductStock);
                PrintUnAvilableSuppliers(purchaseRequirement, unavailableProductStock);

                if (purchaseOrder != null)
                    Console.WriteLine($"Order placed with {purchaseOrder.SupplierName} for {purchaseOrder.QuantityOrdered} {purchaseOrder.ProductName} \n");
                else
                    Console.WriteLine("Order could not be placed due to unavailability of stock \n");

                Console.WriteLine("***********************************************\n");
            }

        }

        /// <summary>
        /// Prints the availble supplier, product details on to the console
        /// </summary>
        /// <param name="purchaseRequirement"></param>
        /// <param name="productStocks"></param>
        private void PrintAvilableSuppliers(PurchaseRequirement purchaseRequirement, List<ProductStock> productStocks)
        {
            SortSuppliers(purchaseRequirement, productStocks);
        }

        /// <summary>
        /// Sort the available suppliers based on total cost and select the supplier who can supply required quantity with minimum cost
        /// </summary>
        /// <param name="purchaseRequirement"></param>
        /// <param name="productStocks"></param>
        private void SortSuppliers(PurchaseRequirement purchaseRequirement, List<ProductStock> productStocks)
        {
            //List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            //set the purchase order to null if no suppliers available for the product
            if (productStocks.Count == 0)
            {
                purchaseOrder = null;
                return;
            }

            //Sort the stock list based on the total cost including shipping
            productStocks = productStocks.OrderBy(l => CalculateTotalCostWithShipping(l.Cost, purchaseRequirement.Quantity, l.Supplier)).ToList();

            foreach (ProductStock productStock in productStocks)
            {

                var totalCostWithShipping = CalculateTotalCostWithShipping(productStock.Cost, purchaseRequirement.Quantity, productStock.Supplier);

                Console.WriteLine($"Product Cost : {productStock.Cost} \nStockOnHand : {productStock.StockOnHand}");
                Console.WriteLine($"Supplier Name : {productStock.Supplier.Name}");
                Console.WriteLine($"Total cost including shipping: {totalCostWithShipping} \n");

                //Because we have already sorted the list based on TotalCost, we can place purchase order from the first item in the list
                if (productStocks.First() == productStock)
                {
                    purchaseOrder = new PurchaseOrder
                    {
                        QuantityOrdered = purchaseRequirement.Quantity,
                        TotalCost = totalCostWithShipping,
                        SupplierName = productStock.Supplier.Name,
                        ProductName = productStock.Product.Name
                    };

                    // purchaseOrders.Add(purchaseOrder);
                }
            }
        }

        /// <summary>
        /// Print the unavailable  supplier, product details on to the console
        /// </summary>
        /// <param name="purchaseRequirement"></param>
        /// <param name="productStocks"></param>
        private void PrintUnAvilableSuppliers(PurchaseRequirement purchaseRequirement, List<ProductStock> productStocks)
        {
            foreach (ProductStock productStock in productStocks)
            {
                //var totalCostWithShipping = CalculateTotalCostWithShipping(productStock.Cost, purchaseRequirement.Quantity, productStock.Supplier);

                Console.WriteLine($"Product Cost : {productStock.Cost} \nStockOnHand : {productStock.StockOnHand}");
                Console.WriteLine($"Supplier Name : {productStock.Supplier.Name}\n");
                //Console.WriteLine($"Total cost including shipping: {totalCostWithShipping} \n");
            }
        }

        /// <summary>
        /// Calculate the total cost of the purchase order including shipping considering the minimum order value & maximum order value
        /// </summary>
        /// <param name="Cost"></param>
        /// <param name="Quantity"></param>
        /// <param name="supplier"></param>
        /// <returns></returns>
        private decimal CalculateTotalCostWithShipping(decimal Cost, int Quantity, Supplier supplier)
        {
            var totalcost = Cost * Quantity;
            var shipping = 0m;

            if (totalcost < supplier.ShippingCostMinOrderValue || totalcost > supplier.ShippingCostMaxOrderValue)
                shipping += supplier.ShippingCost;

            totalcost += shipping;

            return totalcost;
        }

    }
}
