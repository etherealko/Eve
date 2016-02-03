using System;

namespace eth.Common
{
    public interface IHttpClientTimeout
    {
        TimeSpan HttpClientTimeout { get; set; }
    }
}
