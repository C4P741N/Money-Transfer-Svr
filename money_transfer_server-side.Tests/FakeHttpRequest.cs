using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace money_transfer_server_side.Tests
{
    internal class FakeHttpRequest: HttpRequest
    {
        public override HttpContext HttpContext { get; }
        public override string Method { get; set; }
        public override string Scheme { get; set; }
        public override bool IsHttps { get; set; }
        public override HostString Host { get; set; }
        public override PathString PathBase { get; set; }
        public override PathString Path { get; set; }
        public override QueryString QueryString { get; set; }
        public override IQueryCollection Query { get; set; }
        public override string Protocol { get; set; }
        public override IHeaderDictionary Headers { get; } = new HeaderDictionary();
        public override IRequestCookieCollection Cookies { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override Stream Body { get; set; }
        public override bool HasFormContentType { get; }
        public override IFormCollection Form { get; set; }
        public bool HasJsonContentType { get; }
        public IFormCollection ReadForm()
        {
            throw new NotImplementedException();
        }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
