using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NotadogApi.Domain.Exceptions
{
    public class CommonError
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ErrorCode Code { get; }
        public CommonError(ErrorCode code)
        {
            Code = code;
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}

