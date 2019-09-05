namespace NotadogApi.Domain.Users.Models
{
    public class UserUpdatePayload
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
