using JakubKalinaLab7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JakubKalinaLab7.Services
{
    public class PizzaService : IPizzaService
    {
        /// <summary>
        /// Baza danych z pizzami
        /// </summary>
        private static List<Pizza> pizzas = new List<Pizza>()
        {
            new Pizza()
            {
                Id = 0,
                Name = "Wiejska",
                Description = "Pyszna",
                Cost = 19
            },
            new Pizza()
            {
                Id = 1,
                Name = "Wege",
                Description = "Zdrowa",
                Cost = 22
            }
        };

        public List<Pizza> Get()
        {
            return pizzas;
        }

        public int Post(Pizza pizza)
        {
            int id;
            if (pizzas.Count() == 0)
            {
                id = 0;
            }
            else
            {
                id = pizzas.Max(x => x.Id) + 1;
            }

            pizza.Id = id;
            pizzas.Add(pizza);

            return id;
        }

        public bool Put(Pizza pizza, int Id)
        {
            var pizzaToUpdate = pizzas.Where(x => x.Id.Equals(Id)).SingleOrDefault();
            if( pizzaToUpdate == null)
            {
                return false;
            }

            pizzaToUpdate.Name = pizza.Name;
            pizzaToUpdate.Description = pizza.Description;
            pizzaToUpdate.Cost = pizza.Cost;

            return true;
        }

        public bool Delete(int id)
        {
            var pizzaToDelete = pizzas.Where(x => x.Id.Equals(id)).SingleOrDefault();
            if (pizzaToDelete == null)
                return false;

            pizzas.Remove(pizzaToDelete);
            return true;
        }

        public Pizza Get(int id)
        {
            foreach (var pizza in pizzas)
            {
                if (pizza.Id == id)
                    return pizza;
            }
            return null;
        }
    }
}
