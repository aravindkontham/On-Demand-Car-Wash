using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Models;

namespace CarWash.Interfaces
{
    public interface ICarRepository
    {
        Task<Car> AddCarAsync(Car car);
        Task<Car?> GetCarByIdAsync(int id);
        Task<bool> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(int id);
    }
}