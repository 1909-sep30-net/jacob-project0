using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatStore.Logic
{
    struct Component
    {
        public double Cost { get; set; }
        public string Name { get; set; }
    }

    struct Recipe
    {
        Product endProduct;
        Dictionary<Component, int> ingredients;
    }

    class Product
    {
        public Recipe Recipe { get; set; }

        public double Cost 
        { 
            get
            {
                double sum = 0;
                foreach (Component c in components)
                {
                    sum += c.Cost;
                }

                return sum;
            }
        }

        public string Name { get; set; }
    }
}
