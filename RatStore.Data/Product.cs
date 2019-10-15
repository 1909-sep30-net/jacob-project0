using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatStore.Data
{
    public class Product
    {
        public int ProductId { get; set; }

        virtual public List<ProductComponent> Ingredients { get; set; }

        public decimal Cost 
        { 
            get
            {
                decimal sum = 0;
                foreach (ProductComponent c in Ingredients)
                {
                    sum += c.Component.Cost*c.Quantity ?? throw new ArgumentNullException($"{c.Component} has null cost");
                }

                return sum;
            }
        }

        public string Name { get; set; }

        public Product()
        {

        }
        public Product(string name, List<ProductComponent> ingredients)
        {
            Name = name;
            Ingredients = ingredients;
        }
    }
}
