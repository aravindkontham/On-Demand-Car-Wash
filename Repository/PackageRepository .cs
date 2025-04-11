using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Data;
using CarWash.Interfaces;
using CarWash.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWash.Repository
{
   public class PackageRepository : IPackageRepository
{
    private readonly GreenWashDbContext _context;

    public PackageRepository(GreenWashDbContext context)
    {
        _context = context;
    }

    public async Task<Package> AddPackageAsync(Package package)
    {
        _context.Packages.Add(package);
        await _context.SaveChangesAsync();
        return package;
    }

    public async Task<bool> UpdatePackageAsync(Package package)
    {
        _context.Packages.Update(package);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeletePackageAsync(int id)
    {
        var package = await _context.Packages.FindAsync(id);
        if (package == null) return false;

        _context.Packages.Remove(package);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Package>> GetAllPackagesAsync()
    {
        return await _context.Packages.ToListAsync();
    }

    public async Task<Package?> GetPackageByIdAsync(int id)
    {
        return await _context.Packages.FindAsync(id);
    }
}

}