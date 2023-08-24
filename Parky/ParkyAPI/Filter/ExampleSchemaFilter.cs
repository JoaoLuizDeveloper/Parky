using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ParkyAPI.Models.DTOs;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace ParkyAPI.Filter
{
    public class ExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(NationalParkCreateDto))
            {
                schema.Example = new OpenApiObject()
                {
                    ["Name"] = new OpenApiString("Park Name"),
                    ["State"] = new OpenApiString("RJ"),
                    ["Picture"] = new OpenApiNull(),
                    ["Created"] = new OpenApiDate(DateTime.Now),
                    ["Established"] = new OpenApiDate(DateTime.Now)
                };
            }
        }
    }
}
