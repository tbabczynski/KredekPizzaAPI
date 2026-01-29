using JakubKalinaLab7.Models;
using System.Collections.Generic;
using System.Linq;
using JakubKalinaLab7.Extensions;
using Microsoft.AspNetCore.Http;

namespace JakubKalinaLab7.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PizzaService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Baza danych z pizzami
        /// </summary>
        private static readonly List<Pizza> pizzas = new List<Pizza>()
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

        private List<Pizza> GetOrCreatePizzasFromSession()         {
            var session = _httpContextAccessor.HttpContext.Session;
            var pizzas = session.GetObjectFromJson<List<Pizza>>("mypizzas");
            if (pizzas == null)
            {
                pizzas = new List<Pizza>(PizzaService.pizzas);
                session.SetObjectAsJson("mypizzas", pizzas);
            }
            return pizzas;
        }

        public List<Pizza> Get()
        {
            return GetOrCreatePizzasFromSession();
        }

        public int Post(Pizza pizza)
        {
            int id;
            var pizzas = GetOrCreatePizzasFromSession();

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
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetObjectAsJson("mypizzas", pizzas);

            return id;
        }

        public bool Put(Pizza pizza, int Id)
        {
            var pizzas = GetOrCreatePizzasFromSession();

            var pizzaToUpdate = pizzas.Where(x => x.Id.Equals(Id)).SingleOrDefault();
            if( pizzaToUpdate == null)
            {
                return false;
            }

            pizzaToUpdate.Name = pizza.Name;
            pizzaToUpdate.Description = pizza.Description;
            pizzaToUpdate.Cost = pizza.Cost;

            var session = _httpContextAccessor.HttpContext.Session;
            session.SetObjectAsJson("mypizzas", pizzas);

            return true;
        }

        public bool Delete(int id)
        {
            var pizzas = GetOrCreatePizzasFromSession();

            var pizzaToDelete = pizzas.Where(x => x.Id.Equals(id)).SingleOrDefault();
            if (pizzaToDelete == null)
                return false;

            pizzas.Remove(pizzaToDelete);

            var session = _httpContextAccessor.HttpContext.Session;
            session.SetObjectAsJson("mypizzas", pizzas);

            return true;
        }

        public Pizza Get(int id)
        {
            foreach (var pizza in GetOrCreatePizzasFromSession())
            {
                if (pizza.Id == id)
                    return pizza;
            }
            return null;
        }
    }
}
