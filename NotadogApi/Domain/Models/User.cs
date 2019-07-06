using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NotadogApi.Domain.Models
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Email { get; set; }
        public string Password { get; set; }
        [DataMember]
        public int Points { get; set; }
    }
}