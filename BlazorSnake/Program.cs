using GameEngine;
using GameEngine.Sound;
using Howler.Blazor.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSnake
{
    /// <summary>
    /// The main Program class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Starting point of the application
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>A completed task</returns>
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IInputManager, InputManager>();
            builder.Services.AddScoped<IGameTime, GameTime>();
            builder.Services.AddScoped<IGameTimer, GameTimer>();
            builder.Services.AddScoped<IGameObjectDrawer, GameObjectDrawer>();
            builder.Services.AddScoped<ISoundPlayer, SoundPlayer>();

            builder.Services.AddScoped<IHowl, Howl>();
            builder.Services.AddScoped<IHowlGlobal, HowlGlobal>();

            await builder.Build().RunAsync();
        }
    }
}
