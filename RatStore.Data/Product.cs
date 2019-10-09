using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatStore.Data
{
    public struct Component
    {
        public double Cost { get; set; }
        public string Name { get; set; }
    }

    public struct Recipe
    {
        public string EndProductName { get; set; }
        public Dictionary<Component, int> Ingredients { get; set; }
    }

    public class Product
    {
        public Recipe Recipe { get; set; }

        public double Cost 
        { 
            get
            {
                double sum = 0;
                foreach (Component c in Recipe.Ingredients.Keys)
                {
                    sum += c.Cost*Recipe.Ingredients[c];
                }

                return sum;
            }
        }

        public string Name
        {
            get
            {
                return Recipe.EndProductName;
            }
        }
    }
}
