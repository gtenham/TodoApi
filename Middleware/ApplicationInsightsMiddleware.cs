using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;

namespace TodoApi.Middleware 
{

    public class ApplicationInsightsMiddleware 
    {
        private readonly RequestDelegate _next;
        private TelemetryClient _telemetry;
        private readonly ILogger _logger;
        private string _incidentId;
        
        public ApplicationInsightsMiddleware(RequestDelegate next, ILogger<ApplicationInsightsMiddleware> logger)
        {
            _next = next;
            _telemetry = new TelemetryClient();
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Our last line of defense is called.");
                
                _incidentId = null;

                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            catch (Exception e)
            {
                // Any unhandled exception thrown will be handled here!
                _incidentId = generateIncidentId();
                
                // Custom exception tracker
                _telemetry.TrackException(e,
                    new Dictionary<string,string> { {"IncidentID", _incidentId} });

            } 
            finally
            {
                if (_incidentId != null)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"IncidentId\":\"" + _incidentId + "\"}");
                    
                }
            }

        }

        private string generateIncidentId()
        {
            return Guid.NewGuid().ToString().Substring(0,8) + "-" + Guid.NewGuid().ToString().Substring(0,8);
        }
    }
}