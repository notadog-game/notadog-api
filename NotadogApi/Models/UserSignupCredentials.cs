using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NotadogApi.Models
{
    public class UserSignupCredentials : UserLoginCredentials
    {
        public string Name { get; set; }
    }
}
