using System;
using System.Runtime.Serialization;

namespace KeySndr.Common
{
    [DataContract]
    public class ApiResult<T>
        where T : class
    {
        [DataMember(Name="success")]
        public bool Success { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        [DataMember(Name = "content")]
        public T Content { get; set; }

        public ApiResult()
        {
            Success = false;
            Message = string.Empty;
            ErrorMessage = string.Empty;
            Content = default(T);
        }
    }
}
