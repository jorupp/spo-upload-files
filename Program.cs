using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .Build();

var config = host.Services.GetService<IConfiguration>()!;

var site = config["Site"];
var libTitle = config["DocumentLibraryTitle"];
var clientId = config["ClientId"];
var clientSecret = config["ClientSecret"];

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
