using MiddlewareLogin.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseVerifyLoginCreds();

app.Run();
