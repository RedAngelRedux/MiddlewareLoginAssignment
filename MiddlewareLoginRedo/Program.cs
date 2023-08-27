using MiddlewareLoginRedo.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseVerifyLoginCreds();

app.Run(async context => {
    await context.Response.WriteAsync("No response");
});

app.Run();
