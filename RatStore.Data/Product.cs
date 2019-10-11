using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatStore.Data
{
    public class Product
    {
        public Product(string name, Dictionary<Component, int> ingredients)
        {
            Name = name;
            Ingredients = ingredients;
        }

        public Dictionary<Component, int> Ingredients { get; set; }

        public double Cost 
        { 
            get
            {
                double sum = 0;
                foreach (Component c in Ingredients.Keys)
                {
                    sum += c.Cost*Ingredients[c];
                }

                return sum;
            }
        }

        public string Name { get; set; }
    }
}
