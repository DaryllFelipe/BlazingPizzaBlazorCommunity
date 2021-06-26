using BlazingPizza.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Pages
{
    
    public partial class Index
    {
        #region services
        [Inject]
        public HttpClient HttpClient { get; set; }

        #endregion

        #region variables
        List<PizzaSpecial> Specials;
        Pizza ConfiguringPizza;
        bool ShowingConfigureDialog;
        Order Order = new();
        #endregion

        #region overrides
        protected async override Task OnInitializedAsync()
        {
            Specials = await HttpClient.GetFromJsonAsync<List<PizzaSpecial>>("specials");
        }

        #endregion

        #region Methods
        void ShowConfigurePizzaDialog(PizzaSpecial special)
        {
            ConfiguringPizza = new()
            {
                Special = special,
                SpecialId = special.Id,
                Size = Pizza.DefaultSize,
                Toppings = new ()
            };

            ShowingConfigureDialog = true;
        }
        #endregion

        #region Event Manager
        void CancelConfigurePizzaDialog()
        {
            ConfiguringPizza = null;
            ShowingConfigureDialog = false;
        }

        void ConfirmConfigurePizzaDialog()
        {
            Order.Pizzas.Add(ConfiguringPizza);
            ConfiguringPizza = null;
            ShowingConfigureDialog = false;
        }


        void RemoveConfigurePizza(Pizza pizza)
        {
            Order.Pizzas.Remove(pizza);
        }

        async Task PlaceOrder()
        {
            await HttpClient.PostAsJsonAsync("orders", Order);
            Order = new Order();
        }
        #endregion
    }
}
