using JakubKalinaLab7.Models;
using JakubKalinaLab7.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JakubKalinaLab7.Controllers
{
    /// <summary>
    /// Główny punkt wejścia
    /// </summary>
    [Route("api/[controller]")]
    //[EnableCors("AllowPwr")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        // ctor + tab + tab
        /// <inheritdoc />
        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        /// <summary>
        /// Wyświetla podpowiedź
        /// </summary>
        /// <returns>krótka instrukcja co można zrobić</returns>
        [HttpGet]
        [Route("/")]
        [Route("")]
        [Route("help")]
        [Produces("text/html")]
        public IActionResult Instr()
        {
            return base.Content(
                "First try to look in the <a href=\"/api/pizza/menu\">menu</a>, the <a href=\"/index.html\">poor frontend</a>"
                + " or a <a href=\"/swagger\">swagger</a> documentation.", "text/html");
        }

        /// <summary>
        /// Zwraca wszystkie pizze
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("menu")]
        [Produces("application/json")]
        public IActionResult Get()
        {
            var pizzas = _pizzaService.Get();
            return Ok(pizzas);
        }

        /// <summary>
        /// Zwraca pizzę o zadanym identyfikatorze lub informację o braku
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/{id:int}")]
        public IActionResult Order([FromRoute] int id)
        {
            var pizza = _pizzaService.Get(id);

            if (pizza is null)
                return NotFound($"Nie ma takiej pizzy w menu (id: {id})");

            return Ok(pizza);
        }

        /// <summary>
        /// Dodaje nową pizzę
        /// </summary>
        /// <param name="pizza"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] Pizza pizza)
        {
            int id = _pizzaService.Post(pizza);
            return Created($"/api/pizza/{id}", pizza);
        }

        /// <summary>
        /// Edytuje istniejącą pizzę
        /// </summary>
        /// <param name="pizza"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public IActionResult Put([FromBody] Pizza pizza, [FromRoute] int id)
        {
            if (id != pizza.Id)
            {
                return Conflict("Podane Id są różne");
            }
            else
            {
                var isUpdateSuccessful = _pizzaService.Put(pizza, id);

                if (isUpdateSuccessful)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
        }

        /// <summary>
        /// Usuwa pizzę o zadanym identyfikatorze
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var result = _pizzaService.Delete(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Resetuje listę pizz w menu (do dwóch pizz)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Remove("mypizzas");
            return NoContent();

        }
    }
}
