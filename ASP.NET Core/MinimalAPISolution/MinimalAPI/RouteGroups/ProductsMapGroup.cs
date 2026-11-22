// <copyright file="ProductsMapGroup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MinimalAPI.RouteGroups
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Validation;
    using MinimalAPI.EndPointFilters;
    using MinimalAPI.Models;

    public static class ProductsMapGroup
    {
        private readonly static List<Product> products =
            [
                new() { Id = 1, ProductName = "Smart Phone" },
                new() { Id = 2, ProductName = "Smart TV" },
            ];

        public static RouteGroupBuilder ProductsAPI(this RouteGroupBuilder group)
        {
            // Get /porducts
            group.MapGet("/", async (HttpContext context) =>
            {
                await context.Response.WriteAsync(JsonSerializer.Serialize(products));
            });

            // GET/products/{id}
            group.MapGet("/{id:int}", async (HttpContext context, int id) =>
            {
                var product = products.FirstOrDefault(temp => temp.Id == id);
                if (product == null)
                {
                    context.Response.StatusCode = 400; // Bad request
                    await context.Response.WriteAsync("Incorrect Product ID");
                    return;
                }

                await context.Response.WriteAsync(JsonSerializer.Serialize(product));
            });

            group.MapPost("/", async (HttpContext context, Product product) =>
            {
                products.Add(product);

                // await context.Response.WriteAsync("Product Added");
                // Sample Response:
                // { "message": "Product Added"}
                return Results.Ok(new { message = "Product Added" });
            })
            .AddEndpointFilter<CustomEndPointFilter>()
            .AddEndpointFilter(async (context, next) =>
            {
                var product = context.Arguments.OfType<Product>().FirstOrDefault();
                if (product == null)
                {
                    return Results.BadRequest("Product details are not found in the request.");
                }

                var validationContext = new ValidationContext(product);
                var errors = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(product, validationContext, errors, true);

                if (!isValid)
                {
                    return Results.BadRequest(errors.FirstOrDefault()?.ErrorMessage);
                }

                var result = await next(context); // Invokes the subsequent endpoint filter or endpoint request delegate

                // After logic here
                return result;
            });

            // PUT /products/{id}
            group.MapPut("/{id}", async (HttpContext context, int id, [FromBody] Product product) =>
            {
                var productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
                if (productFromCollection == null)
                {
                    context.Response.StatusCode = 400; // Bad request
                    await context.Response.WriteAsync("Incorrect Product ID");
                    return;
                }

                productFromCollection.Id = product.Id;
                productFromCollection.ProductName = product.ProductName;
                await context.Response.WriteAsync("Product Updated");
            });

            // DELETE /products/{id}
            group.MapDelete("/{id}", async (HttpContext context, int id) =>
            {
                var productFromCollection = products.FirstOrDefault(temp => temp.Id == id);
                if (productFromCollection == null)
                {
                    // context.Response.StatusCode = 400; // Bad request
                    // await context.Response.WriteAsync("Incorrect Product ID");
                    // return;
                    // return Results.BadRequest(new { error = "Incorrect Product ID" });
                    return Results.ValidationProblem(
                        new Dictionary<string, string[]>()
                        {
                            { "id", new string[] { "Incorrect Product ID" } },
                        });
                }

                products.Remove(productFromCollection);

                // await context.Response.WriteAsync("Product Deleted");
                return Results.Ok(new { message = "Product Deleted" });
            });

            return group;
        }
    }
}
