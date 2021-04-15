using AspNetCoreLogging.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CarsController> logger;

        public CarsController(ICarsService carsService, ILogger<CarsController> logger)
        {
            this.carsService = carsService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<List<Cars>> Get()
        {
            string temp1 = "test string";

            logger.LogInformation($"Information Log {temp1}");
            logger.LogWarning($"Information Log {temp1}");
            logger.LogError($"Information Log {temp1}");
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
            throw new NullReferenceException();
            return await carsService.GetCarById(id, make);
        }
    }
}
