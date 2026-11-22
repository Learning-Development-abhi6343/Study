// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Models;
using MinimalAPI.RouteGroups;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// List<Product> products =
//    [
//        new() { Id = 1, ProductName = "Smart Phone" },
//        new() { Id = 2, ProductName = "Smart TV" },
//    ];

// var mapGroup = app.MapGroup("/products");
var group = app.MapGroup("/products").ProductsAPI();

// Get /porducts
// app.MapGet("/products", async (HttpContext context) =>
// mapGroup.MapGet("/", async (HttpContext context) =>
// {
//    // var content = string.Join('\n', products.Select(temp => temp.ToString()));
//    // await context.Response.WriteAsync(content);
//    await context.Response.WriteAsync(JsonSerializer.Serialize(products));
// });

////app.MapGet("/", async (HttpContext context) =>
////{
////    await context.Response.WriteAsync("GET - Hello World");
////});

//// GET/products/{id}
//// app.MapGet("/products/{id:int}", async (HttpContext context, int id) =>
// mapGroup.MapGet("/{id:int}", async (HttpContext context, int id) =>
// {
//    var product = products.FirstOrDefault(temp => temp.Id == id);
//    if (product == null)
//    {
//        context.Response.StatusCode = 400; // Bad request
//        await context.Response.WriteAsync("Incorrect Product ID");
//        return;
//    }

    // await context.Response.WriteAsync(product!.ToString());
//    await context.Response.WriteAsync(JsonSerializer.Serialize(product));

// });

//// app.MapPost("/products", async (HttpContext context, Product product) =>
// mapGroup.MapPost("/", async (HttpContext context, Product product) =>
// {
//    products.Add(product);
//    await context.Response.WriteAsync("Product Added");
// });
app.Run();
