using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.Interfaces
{
    public interface ILogService
    {
        Task LogAsync(string level, string message, string? exception = null, int? userId = null);
    }
}