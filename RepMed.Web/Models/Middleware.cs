using RepMed.Core;
using RepMed.Dtos;
using RepMed.Web.logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepMed.Web.Models
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            try
            {
                context.Request.EnableBuffering();    //add this...             
                await _next(context);
                context.Request.Body.Position = 0;    //add this...
            }
            finally
            {
                
                var response = context.Response;
                if (response.StatusCode != StatusCodes.Status200OK)
                {
                    var responseObj = new APIsResponse<object>
                    {
                        //essage = error?.GetActualError(),
                        IsSuccess = false,
                        Data = null,
                        //Status = (int)HttpStatusCode.InternalServerError
                    };

                    switch (context.Response?.StatusCode)
                    {
                        case StatusCodes.Status404NotFound:
                            responseObj.Status = StatusCodes.Status404NotFound;
                            break;
                        case StatusCodes.Status401Unauthorized:
                            responseObj.Status = StatusCodes.Status401Unauthorized;
                            break;
                        case StatusCodes.Status500InternalServerError:
                            responseObj.Status = StatusCodes.Status500InternalServerError;
                            break;
                        case StatusCodes.Status400BadRequest:
                            responseObj.Status = StatusCodes.Status400BadRequest;
                            break;
                        default:
                            break;
                    }

                    var result = JsonConvert.SerializeObject(responseObj);
                    await response.WriteAsync(result);
                }
            }

            //try
            //{
            //    await _next(context);
            //}
            //catch (Exception error)
            //{
            //    var response = context.Response;
            //    response.ContentType = "application/json";

            //    //switch (error)
            //    //{
            //    //    case AppException e:
            //    //        // custom application error
            //    //        response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    //        break;
            //    //    case KeyNotFoundException e:
            //    //        // not found error
            //    //        response.StatusCode = (int)HttpStatusCode.NotFound;
            //    //        break;
            //    //    default:
            //    //        // unhandled error
            //    //        response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //    //        break;
            //    //}
            //    response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //    var result = JsonConvert.SerializeObject(new APIsResponse<object>
            //    {
            //        Message = error?.GetActualError(),
            //        IsSuccess = false,
            //        Data = null,
            //        Status = (int)HttpStatusCode.InternalServerError
            //    });
            //    await response.WriteAsync(result);
            //}
        }
    }

    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var message = contextFeature.Error.GetActualError();
                        //logger.LogError($"Something went wrong: {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = message
                        }.ToString());
                    }
                });
            });
        }
    }

    /// <summary>
    /// Customize BadRequest Response
    /// </summary>
    public class CustomBadRequest : ValidationProblemDetails
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomBadRequest()
        {
        }

        /// <summary>
        /// success
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        /// <summary>
        /// message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Ctr
        /// </summary>
        /// <param name="context"></param>
        /// 
        public CustomBadRequest(ActionContext context)
        {
            Success = false;
            Title = "Invalid arguments to the API";
            Status = StatusCodes.Status500InternalServerError;
            ConstructErrorMessages(context);
            Type = context.HttpContext.TraceIdentifier;
            Instance = $"urn:myorganization:badrequest:{Guid.NewGuid()}";
        }
        private void ConstructErrorMessages(ActionContext context)
        {
            try
            {
                Message = context.ModelState.GetError();
            }
            catch (Exception ex)
            {
               // RELogger.LogError(ex, Directory.GetCurrentDirectory());
            }
        }

        private string GetErrorMessage(Microsoft.AspNetCore.Mvc.ModelBinding.ModelError error)
        {
            try
            {
                return string.IsNullOrEmpty(error.ErrorMessage) ? "The input was not valid." : error.ErrorMessage;
            }
            catch (Exception ex)
            {
               // RELogger.LogError(ex, Directory.GetCurrentDirectory());
            }
            return null;
        }
    }

}
