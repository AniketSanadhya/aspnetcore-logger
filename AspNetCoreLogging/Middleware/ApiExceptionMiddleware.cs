using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AspNetCoreLogging.Middleware
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ApiExceptionMiddleware> logger;

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {

            Dictionary<string, string> customProperties = new Dictionary<string, string>();
            TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.InstrumentationKey = "key";

            TelemetryClient telemetryClient = new TelemetryClient(telemetryConfiguration);

            var error = new ApiError()
            {
                Id = Guid.NewGuid().ToString(),
                Status = HttpStatusCode.InternalServerError
            };

            var innerMessage = GetInnerMessage(ex);

            logger.LogError(ex, "Inner Message: " + innerMessage + " Id: " + error.Id);
            customProperties.Add("Inner Message", innerMessage);
            customProperties.Add("id", error.Id);
            telemetryClient.TrackException(ex, customProperties);

            var result = JsonConvert.SerializeObject(error);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return httpContext.Response.WriteAsync(result);
        }

        private string GetInnerMessage(Exception ex)
        {
            if(ex.InnerException != null)
            {
                GetInnerMessage(ex.InnerException);
            }
            return ex.Message;
        }
    }
}
