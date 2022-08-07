using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeanSongs.Exceptions
{
    public class ApiServiceException : Exception
    {
        public HttpResponseMessage? response { get; set; }

        public ApiServiceException(string message, HttpResponseMessage? response)
            : base($"StatusCode: {response?.StatusCode}, Message: {message}")
        {
            this.response = response;
        }
    }
}
