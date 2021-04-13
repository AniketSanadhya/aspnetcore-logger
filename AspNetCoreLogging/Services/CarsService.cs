using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreLogging.Services
{
    public class CarsService : ICarsService
    {
        private Container container;
        public CarsService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task Add(Cars car)
        {
            car.Id = Guid.NewGuid();
            await container.CreateItemAsync(car, new PartitionKey(car.Make));
        }

        public async Task Delete(Guid id, string make)
        {
            await container.DeleteItemAsync<Cars>(id.ToString(), new PartitionKey(make));
        }

        public async Task<List<Cars>> GetCars()
        {
            try
            {
                var response = container.GetItemQueryIterator<Cars>(new QueryDefinition("SELECT * FROM Cars"));
                List<Cars> cars = new List<Cars>();
                while (response.HasMoreResults)
                {
                    var data = await response.ReadNextAsync();
                    cars.AddRange(data.ToList());
                }
                return cars;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Cars> GetCarById(Guid id, string make)
        {
            try
            {
                var response = await container.ReadItemAsync<Cars>(id.ToString(), new PartitionKey(make));
                return response.Resource;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task Update(Guid id, Cars car)
        {
            try
            {

                var data = await GetCarById(id, car.Make);
                if (data != null)
                {
                    await container.UpsertItemAsync<Cars>(car, new PartitionKey(car.Make));
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
