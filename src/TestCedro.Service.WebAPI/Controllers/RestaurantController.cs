using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces.Services;

namespace TestCedro.Service.WebAPI.Controllers
{
    /// <summary>
    /// Controller used to insert, update and get the restaurant information
    /// </summary>
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        /// <summary>
        /// Construtor method
        /// </summary>
        /// <param name="restaurantService">Restaurant service, used to manipulate the dishes information</param>
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        /// <summary>
        /// Method used to get the restaurant information by id.
        /// </summary>
        /// <param name="id">Restaurant unique id</param>
        ///<returns></returns>
        //GET v1/restaurant/40404403-c66a-41ee-8bc4-bd107fb85032
        [Route("{id:guid}")]
        [HttpGet]
        public IActionResult Get(Guid id)
        {
            try
            {
                return Ok(_restaurantService.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //POST v1/restaurant
        /// <summary>
        /// Method used to be created restaurant information
        /// </summary>
        /// <param name="model">Restaurant information to be inserted</param>
        /// <returns>Restaurant information with id generated</returns>
        [Route("")]
        [HttpPost]
        public IActionResult Post([FromBody]Restaurant model)
        {
            try
            {
                return Ok(_restaurantService.Add(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //PUT v1/restaurant
        /// <summary>
        /// Method used to be updated restaurant information
        /// </summary>
        /// <param name="model">Restaurant information to be updated</param>
        /// <returns>Restaurant information updated</returns>
        [Route("")]
        [HttpPut]
        public IActionResult Put([FromBody]Restaurant model)
        {
            try
            {
                return Ok(_restaurantService.Update(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //DELETE v1/restaurant
        /// <summary>
        /// Method used to be deleted restaurant information
        /// </summary>
        /// <param name="id">Restaurant identification</param>
        /// <returns>Ok if the restaurant was deleted</returns>
        [Route("{id:guid}")]
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _restaurantService.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET v1/restaurant/search/estrela
        /// <summary>
        /// Method used to get the list of dishes with specific name
        /// </summary>
        /// <param name="search">Dish's name or part</param>
        /// <returns>Dish information with id generated</returns>
        [Route("search/{search=}")]
        [HttpGet]
        public IActionResult Search(string search = "")
        {
            try
            {
                var clients = _restaurantService.Search(x => x.Name.Contains(search ?? string.Empty))
                    ?.Select(x => new Restaurant
                    {
                        RestaurantId = x.RestaurantId,
                        Name = x.Name
                    }).ToList();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _restaurantService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}