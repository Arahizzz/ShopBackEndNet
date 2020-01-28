using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orders;

        public OrdersController(OrderService orders)
        {
            _orders = orders;
        }

        [Authorize]
        [HttpGet]
        public Task<List<Order>> GetOrders() => _orders.GetOrders();

        [HttpGet("{id:length(24)}", Name = "GetOrder")]
        public async Task<ActionResult<Order>> GetOrder(string id)
        {
            var order = await _orders.GetOrder(id);

            if (order == null)
                return NotFound();

            return order;
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder(Order order)
        {
            if (string.IsNullOrEmpty(order.Name))
                return BadRequest();
            await _orders.AddOrder(order);
            return CreatedAtRoute("GetOrder", new {id = order.Id}, order);
        }

        [Authorize]
        [HttpPatch("{id:length(24)}")]
        public async Task<ActionResult> UpdateOrder(string id, Order orderIn)
        {
            var product = await _orders.GetOrder(id);

            if (product == null)
                return NotFound();

            await _orders.UpdateOrder(id, orderIn);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteOrder(string id)
        {
            var product = await _orders.GetOrder(id);

            if (product == null)
                return NotFound();

            await _orders.DeleteOrder(id);
            return NoContent();
        }
    }
}