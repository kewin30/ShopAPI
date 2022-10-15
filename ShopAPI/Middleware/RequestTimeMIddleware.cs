﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShopAPI.Middleware
{
    public class RequestTimeMIddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMIddleware> _logger;
        private Stopwatch _stopWatch;

        public RequestTimeMIddleware(ILogger<RequestTimeMIddleware> logger)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            var elapsedMiliseconds = _stopWatch.ElapsedMilliseconds;
            if(elapsedMiliseconds/1000 >4)
            {
                var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMiliseconds} ms";
                _logger.LogInformation(message);
            }
        }
    }
}
