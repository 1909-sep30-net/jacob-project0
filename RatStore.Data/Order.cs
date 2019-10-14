using System;
using System.Collections.Generic;
using System.Text;

namespace RatStore.Data
{
    public class Order
    {
        public Order()
        {
            OrderDate = DateTime.Now;
            Id = -1;
        }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }

        public decimal Const 
        { 
            get
            {
                decimal sum = 0;
                foreach (OrderDetails orderDetails in OrderDetails)
                {
                    sum += orderDetails.Product.Cost * orderDetails.Quantity;
                }

                return sum;
            }
        }
    }
}
