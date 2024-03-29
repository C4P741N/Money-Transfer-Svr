﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace money_transfer_server_side.Tests
{
    internal class FakeHttpResponse : HttpResponse
    {
        public override HttpContext HttpContext { get; }
        public override int StatusCode { get; set; }
        public override IHeaderDictionary Headers { get; } = new HeaderDictionary();
        public override Stream Body { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override IResponseCookies Cookies { get; }
        public override bool HasStarted { get; }
        public override void OnStarting(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }
        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            throw new NotImplementedException();
        }
        public override void Redirect(string location, bool permanent)
        {
            throw new NotImplementedException();
        }
        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
