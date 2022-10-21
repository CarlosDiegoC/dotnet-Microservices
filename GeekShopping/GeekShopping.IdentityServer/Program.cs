using GeekShopping.IdentityServer;
using GeekShopping.IdentityServer.Configuration.Initializer;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

var dbInitializeService = app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>();

startup.Configure(app, app.Environment, dbInitializeService);

app.Run();
