using JakubKalinaLab7.Models;
using JakubKalinaLab7.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JakubKalinaLab7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        // ctor + tab + tab
        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        /// <summary>
        /// Wyświetla podpowiedź
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Instr()
        {
            return base.Content(content: "Try to look in the <b>menu</b> first", contentType: "text/html");
        }

        /// <summary>
        /// Zwraca wszystkie pizze
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("menu")]
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
        [HttpGet, Route("order/{id}.{format?}")]
        public IActionResult Order([FromRoute] string id)
        {
            Pizza pizza;
            try
            {
                pizza = _pizzaService.Get(int.Parse(id));
            }
            catch (Exception)
            {
                return BadRequest("Id must be a number");
            }

            if (pizza is null)
                return NotFound("Nie ma takiej pizzy w menu");
            else
            {
                return Ok(pizza);
            }
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
            return Ok(id);
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

    }
}
