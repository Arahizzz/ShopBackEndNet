using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Models;
using DemoAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _products;

        public ProductsController(ProductService service)
        {
            _products = service;
        }

        [HttpGet]
        public Task<List<Product>> GetProducts() =>  _products.GetProducts();

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _products.GetProduct(id);

            if (product == null)
                return NotFound();

            return product;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddProduct(Product product)
        {
            if (string.IsNullOrEmpty(product.Name) || product.Price <= 0)
                return BadRequest();
            await _products.AddProduct(product);
            return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
        }

        [Authorize]
        [HttpPatch("{id:length(24)}")]
        public async Task<ActionResult> UpdateProduct(string id, Product productIn)
        {
            var product = await _products.GetProduct(id);

            if (product == null)
                return NotFound();

            await _products.UpdateProduct(id, productIn);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            var product = await _products.GetProduct(id);

            if (product == null)
                return NotFound();

            await _products.DeleteProduct(id);
            return NoContent();
        }
    }
}