using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Models;

namespace CarWash.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}