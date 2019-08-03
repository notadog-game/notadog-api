using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NotadogApi.Models
{
    public class UserSignupCredentials
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
