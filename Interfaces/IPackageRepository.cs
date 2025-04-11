using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Models;

namespace CarWash.Interfaces
{
    public interface IPackageRepository
{
    Task<Package> AddPackageAsync(Package package);
    Task<bool> UpdatePackageAsync(Package package);
    Task<bool> DeletePackageAsync(int id);
    Task<IEnumerable<Package>> GetAllPackagesAsync();
    Task<Package?> GetPackageByIdAsync(int id);
}

}