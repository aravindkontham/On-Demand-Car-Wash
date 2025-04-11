using CarWash.Data;
using CarWash.Interfaces;
using CarWash.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly GreenWashDbContext _context;

        public CarRepository(GreenWashDbContext context)
        {
            _context = context;
        }

        public async Task<Car> AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            return await _context.Cars.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return false;
            _context.Cars.Remove(car);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
