using BlazingPizza.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BlazingPizza.Client.Pages
{
    public partial class OrderDetails
    {
        [Parameter]
        public int OrderId { get; set; }

        [Inject]
        public HttpClient HttpClient { get; set; }

        OrderWithStatus OrderWithStatus;

        bool InvalidOrder;

        CancellationTokenSource PollingCancellationToken;


        protected override void OnParametersSet()
        {
            PollingCancellationToken?.Cancel();
            PollForUpdates();
        }

        private async void PollForUpdates()
        {
            PollingCancellationToken = new CancellationTokenSource();
            while (!PollingCancellationToken.IsCancellationRequested)
            {
                try
                {
                    InvalidOrder = false;
                    OrderWithStatus =
                        await HttpClient.GetFromJsonAsync<OrderWithStatus>($"orders/{OrderId}");
                    StateHasChanged();
                    if (OrderWithStatus.IsDelivered)
                    {
                        PollingCancellationToken.Cancel();
                    }
                    else
                    {
                        await Task.Delay(4000);

                    }
                }
                catch (Exception ex)
                {

                    InvalidOrder = true;
                    PollingCancellationToken.Cancel();
                    Console.Error.WriteLine(ex);
                    StateHasChanged();
                }
            }
        }

    }
}
