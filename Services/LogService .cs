using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWash.Data;
using CarWash.Interfaces;
using CarWash.Models;

namespace CarWash.Services
{
   public class LogService : ILogService
{
    private readonly GreenWashDbContext _context;

    public LogService(GreenWashDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(string level, string message, string? exception = null, int? userId = null)
    {
        var log = new Log
        {
            Timestamp = DateTime.UtcNow,
            Level = level,
            Message = message,
            Exception = exception ?? string.Empty,
            UserId = userId
        };

        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }
}

}