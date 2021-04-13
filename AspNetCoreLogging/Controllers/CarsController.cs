using AspNetCoreLogging.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreLogging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly ICarsService carsService;

        public CarsController(ICarsService carsService)
        {
            this.carsService = carsService;
        }

        [HttpGet]
        public async Task<List<Cars>> Get()
        {
            return await carsService.GetCars();
        }

        [HttpPost]
        public async Task Add(Cars car)
        {
            await carsService.Add(car);
        }

        [HttpPut("{id}")]
        public async Task Update(Guid id, Cars car)
        {
            await carsService.Update(id, car);
        }

        [HttpDelete("{id}/{make}")]
        public async Task Delete(Guid id, string make)
        {
            await carsService.Delete(id, make);
        }

        [HttpGet("{id}/{make}")]
        public async Task<Cars> Get(Guid id, string make)
        {
            return await carsService.GetCarById(id, make);
        }
    }
}
