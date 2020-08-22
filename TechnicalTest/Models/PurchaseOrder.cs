using System;
using System.Collections.Generic;
using System.Text;

namespace Gluh.TechnicalTest.Models
{
    class PurchaseOrder
    {
        public string SupplierName { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal TotalCost { get; set; }
        public string ProductName { get; set; }
        //public int SupplierID { get; set; }
    }
}
