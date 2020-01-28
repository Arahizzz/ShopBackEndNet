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
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categories;

        public CategoriesController(CategoryService categories)
        {
            _categories = categories;
        }

        [HttpGet]
        public Task<IEnumerable<Category>> GetCategories() => _categories.GetCategories();

        [HttpGet("{id:length(24)}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> GetCategory(string id)
        {
            var category = await _categories.FetchCategory(id);

            if (category == null)
                return NotFound();

            return category;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
                return BadRequest();
            await _categories.AddCategory(category);
            return CreatedAtRoute("GetCategory", new {id = category.Id}, category);
        }

        [Authorize]
        [HttpPatch("{id:length(24)}")]
        public async Task<ActionResult> UpdateCategory(string id, Category categoryIn)
        {
            var category = await _categories.GetCategory(id);

            if (category == null)
                return NotFound();

            await _categories.UpdateCategory(id, categoryIn);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteCategory(string id)
        {
            var product = await _categories.GetCategory(id);

            if (product == null)
                return NotFound();

            await _categories.DeleteCategory(id);
            return NoContent();
        }
    }
}