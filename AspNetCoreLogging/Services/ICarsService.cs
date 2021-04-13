using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreLogging.Services
{
    public interface ICarsService
    {
        Task Add(Cars car);
        Task Delete(Guid id, string make);
        Task<Cars> GetCarById(Guid id, string make);
        Task<List<Cars>> GetCars();
        Task Update(Guid id, Cars car);
    }
}
