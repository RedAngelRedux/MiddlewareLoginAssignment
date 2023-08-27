namespace MiddlewareLogin.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class VerifyLoginCreds
    {
        private readonly RequestDelegate _next;

        public VerifyLoginCreds(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string method = httpContext.Request.Method;
            string response = string.Empty;

            if (method == "POST")
            {
                string email = httpContext.Request.Query["email"];
                string password = httpContext.Request.Query["password"];


                // Make sure required parameters are present
                response += (email == null) ? "'email' is required\n" : string.Empty;
                response += (password == null) ? "'password' is required\n" : string.Empty;

                // Make sure required parameter contain valid values
                if (response == string.Empty)
                {
                    response += (email != "admin@example.com") ? "Invalid Input for e-mail\n" : string.Empty;
                    response += (password != "admin1234") ? "Invalid Input for password\n" : string.Empty;
                }

                // If response is still empty at this point, no error conditions were encountered
                if (response == string.Empty)
                {
                    response = "\nSuccessful Login\n";
                    httpContext.Response.StatusCode = 200;
                }
                else
                {
                    response += "\nLogin Failed\n";
                    httpContext.Response.StatusCode = 400;
                }
            }
            else
            {
                response = "No Response";
                httpContext.Response.StatusCode = 200;
            }

            // Add response message(s) to the Response
            await httpContext.Response.WriteAsync(response);

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class VerifyLoginCredsExtensions
    {
        public static IApplicationBuilder UseVerifyLoginCreds(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VerifyLoginCreds>();
        }
    }
}
