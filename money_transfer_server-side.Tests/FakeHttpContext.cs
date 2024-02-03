using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace money_transfer_server_side.Tests
{
    public class FakeHttpContext : HttpContext
    {
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;
        public FakeHttpContext()
        {
            _request = new FakeHttpRequest();
            _response = new FakeHttpResponse();
        }
        public override IFeatureCollection Features => new FeatureCollection();

        public override HttpRequest Request => _request;

        public override HttpResponse Response => _response;

        public override ConnectionInfo Connection => throw new NotImplementedException();

        public override WebSocketManager WebSockets => throw new NotImplementedException();

        public override ClaimsPrincipal User { get; set; }
        public override IDictionary<object, object?> Items { get; set; } = new Dictionary<object, object?>();
        public override IServiceProvider RequestServices { get; set; }
        public override CancellationToken RequestAborted { get; set; }
        public override string TraceIdentifier { get; set; }
        public override ISession Session { get; set; }

        public override void Abort()
        {
            throw new NotImplementedException();
        }
    }
}
