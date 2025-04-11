using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWash.DTO
{
       public class ResetPasswordDto
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string VerifyPassword { get; set; }
}
}