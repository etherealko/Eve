using System;
using System.Net.Http;
using eth.Common;
using SocksSharp;
using SocksSharp.Proxy;

namespace eth.Telegram.BotApi.Proxies
{
    public class SocksProxy : IHttpClientProxy
    {
        private readonly ProxySettings _settings;

        public SocksProtocolVersion Version { get; set; } = SocksProtocolVersion.Socks5;

        public SocksProxy(string host, int port, string user = null, string password = null)
        {
            _settings = new ProxySettings();

            _settings.SetHost(host)
                     .SetPort(port)
                     .SetCredential(user, password).SetReadWriteTimeout(60000);
        }

        public HttpMessageHandler CreateMessageHandler()
        {
            switch (Version)
            {
                case SocksProtocolVersion.Socks5:
                    return new ProxyClientHandler<Socks5>(_settings);
                case SocksProtocolVersion.Socks4:
                    return new ProxyClientHandler<Socks4>(_settings);
                case SocksProtocolVersion.Socks4a:
                    return new ProxyClientHandler<Socks4a>(_settings);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum SocksProtocolVersion
    {
        Socks5,
        Socks4,
        Socks4a
    }
}
