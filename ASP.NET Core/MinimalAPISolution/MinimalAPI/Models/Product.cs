// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MinimalAPI.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Required(ErrorMessage = "Id can't be blank")]
        [Range(0, int.MaxValue, ErrorMessage = "Id should be between 0 to maximum value of int")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name can't be blank")]
        public string? ProductName { get; set; }

        public override string ToString()
        {
            return $"Product ID: {this.Id}, Product Name: {this.ProductName}";
        }
    }
}
