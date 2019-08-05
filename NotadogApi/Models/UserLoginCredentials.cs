using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NotadogApi.Models
{
    public class UserLoginCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
