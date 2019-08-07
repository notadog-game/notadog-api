using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NotadogApi.Domain.Models
{
    public class UserUpdatePayload
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
