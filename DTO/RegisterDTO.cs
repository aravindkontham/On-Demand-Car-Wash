namespace CarWash.DTO
{
    public class RegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer"; 
        public string Location { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

   
}
