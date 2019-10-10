using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Order
    {
        public Order()
        {
            OrderTimestamp = DateTime.Now;
        }
        public int OriginStoreId { get; set; }
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderTimestamp { get; set; }
        public Dictionary<Product, int> OrderProducts { get; set; }

        public double Const 
        { 
            get
            {
                double sum = 0;
                foreach (Product product in OrderProducts.Keys)
                {
                    sum += product.Cost * OrderProducts[product];
                }

                return sum;
            }
        }
    }
}
