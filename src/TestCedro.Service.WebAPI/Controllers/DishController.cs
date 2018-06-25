using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestCedro.Domain.Entities;
using TestCedro.Domain.Interfaces.Services;

namespace TestCedro.Service.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class DishController : Controller
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

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
        [Route("{id:guid}")]
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _dishService.Remove(id);
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

        //GET v1/dish/search/estrela
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