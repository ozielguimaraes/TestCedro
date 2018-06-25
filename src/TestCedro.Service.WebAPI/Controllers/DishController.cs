using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces.Services;

namespace TestCedro.Service.WebAPI.Controllers
{
    /// <summary>
    /// Controller used to insert, update and get the dish information
    /// </summary>
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class DishController : Controller
    {
        private readonly IDishService _dishService;
        
        /// <summary>
        /// Construtor method
        /// </summary>
        /// <param name="dishService">Dish service, used to manipulate the dishes information</param>
        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        
        /// <summary>
        /// Method used to get dish information by id
        /// </summary>
        /// <param name="id">Dish identification</param>
        /// <returns>Dish information</returns>
        //GET v1/dish/40404403-c66a-41ee-8bc4-bd107fb85032
        [Route("{id:guid}")]
        [HttpGet]
        public IActionResult Get(Guid id)
        {
            try
            {
                return Ok(_dishService.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //POST v1/dish
        /// <summary>
        /// Method used to be created dish information
        /// </summary>
        /// <param name="model">Dish information to be inserted</param>
        /// <returns>Dish information with id generated</returns>
        [Route("")]
        [HttpPost]
        public IActionResult Post([FromBody]Dish model)
        {
            try
            {
                return Ok(_dishService.Add(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT v1/dish
        /// <summary>
        /// Method used to be updated dish information
        /// </summary>
        /// <param name="model">Dish information to be updated</param>
        /// <returns>Dish information updated</returns>
        [Route("")]
        [HttpPut]
        public IActionResult Put([FromBody]Dish model)
        {
            try
            {
                return Ok(_dishService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETE v1/dish
        /// <summary>
        /// Method used to be deleted dish information
        /// </summary>
        /// <param name="id">Dish identification</param>
        /// <returns>Ok if the dish was deleted</returns>
        [Route("{id:guid}")]
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _dishService.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET v1/dish/search/assado
        /// <summary>
        /// Method used to get the list of dishes with specific name
        /// </summary>
        /// <param name="search">Dish's name or part</param>
        /// <returns>Dish information with id generated</returns>
        [Route("search/{search=}")]
        [HttpGet]
        public IActionResult Search(string search = "")
        {
            var clients = _dishService.Search(x => x.Name.Contains(search ?? string.Empty))
                ?.Select(x => new Dish
                {
                    DishId = x.DishId,
                    Name = x.Name,
                    Value = x.Value,
                });
            return Ok(clients);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dishService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}