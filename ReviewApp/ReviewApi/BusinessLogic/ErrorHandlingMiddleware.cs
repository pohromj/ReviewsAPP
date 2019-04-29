using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReviewApi.BusinessLogic
{
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        /// dalsi middleware
        /// </summary>
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,  IConfiguration configuration)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, configuration["Version"]);
            }
        }

        /// <summary>
        /// handler chyby
        /// </summary>
        /// <param name="helperService">servis pro ulozeni chyb</param>
        /// <param name="context">aktualni HTTP kontext</param>
        /// <param name="exception">zachycena vyjimka</param>
        /// <param name="version">verze aplikace</param>
        /// <returns></returns>
        private static Task HandleExceptionAsync( HttpContext context, Exception exception, string version)
        {
            //Task error = helperService.SaveErrorToDB(new AppErrorModel(exception, $"JazzMetricsAPI - {version} -> {context.User.GetId()}", "global error handler"));

            /*string result = JsonConvert.SerializeObject(new BaseResponseModel
            {
                Message = "Error occured on server within request processing.",
                Success = false
            });*/

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //error.Wait();

            return context.Response.WriteAsync("Something went wrong");
        }
    }
}
