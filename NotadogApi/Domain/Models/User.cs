using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NotadogApi.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [IgnoreDataMember]
        public string Password { get; set; }
        public int Score { get; set; }
    }
}