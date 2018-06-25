using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces.Services;

namespace TestCedro.Service.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

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
        [Route("{id:guid}")]
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _restaurantService.Remove(id);
                return Ok(new
                {
                    message = "Operação realizada com sucesso"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET v1/restaurant/search/estrela
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