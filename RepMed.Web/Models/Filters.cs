using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepMed.Web.Models
{
    public class HeadersFilters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo.GetCustomAttributes(true)
           .OfType<AuthorizeApiAttribute>()
           .Distinct();

            if (operation.Tags.Count > 0 && !HideControllers.Select(r => r.Trim().ToLower()).Contains(operation.Tags.FirstOrDefault().Name.ToLower()))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                //operation.Parameters.Add(new OpenApiParameter
                //{
                //    Name = "Accept-Language",
                //    In = ParameterLocation.Header,
                //    Description = "Please specific languages eg. en,ar",
                //    Required = true,
                //    //Schema = new OpenApiSchema { Type = "string", Format = "select" },
                //    //Example = new OpenApiString("en")




                //});

            }
        }

        private string[] HideControllers
        {
            get { return new string[] { "" }; }
        }
    }
}
