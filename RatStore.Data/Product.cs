using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatStore.Data
{
    public class Product
    {
        public Product()
        {

        }
        public Product(string name, List<ProductComponent> ingredients)
        {
            Name = name;
            Ingredients = ingredients;
        }

        public int Id { get; set; }

        public List<ProductComponent> Ingredients { get; set; }

        public decimal Cost 
        { 
            get
            {
                decimal sum = 0;
                foreach (ProductComponent c in Ingredients)
                {
                    sum += c.Component.Cost*c.Quantity;
                }

                return sum;
            }
        }

        public string Name { get; set; }
    }
}
