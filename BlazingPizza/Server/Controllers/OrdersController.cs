using BlazingPizza.Server.Models;
using BlazingPizza.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingPizza.Server.Controllers
{
    [Route("Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly PizzaStoreContext Context;
        public OrdersController(PizzaStoreContext context)
        {
            Context = context;
        }

        [HttpPost]
        public async Task<ActionResult<int>> PlaceOrder(Order order)
        {
            order.CreatedTime = DateTime.Now;
            // Establecer una ubicación de envío ficticia
            order.DeliveryLocation =
                new LatLong(15.06813771206520357, -98.19811254438645);

            // Establecer el valor de Pizza.SpecialId y Topping.ToppingId
            // para que no se creen nuevos registros Special y Topping.
            foreach (var Pizza in order.Pizzas)
            {
                Pizza.SpecialId = Pizza.Special.Id;
                Pizza.Special = null;

                foreach (var topping in Pizza.Toppings)
                {
                    topping.ToppingId = topping.Topping.Id;
                    topping.Topping = null;
                }
            }

            Context.Orders.Attach(order);
            await Context.SaveChangesAsync();

            return order.OrderId;
        }
    }
}
