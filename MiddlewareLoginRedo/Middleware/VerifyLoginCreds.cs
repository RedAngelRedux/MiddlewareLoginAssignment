using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace MiddlewareLoginRedo.Middleware
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
                //Read response body as stream
                StreamReader reader = new StreamReader(httpContext.Request.Body);
                string body = await reader.ReadToEndAsync();

                //Parse the request body from string into Dictionary
                Dictionary<string, StringValues> queryDict = QueryHelpers.ParseQuery(body);

                // Evaluate the email parameter
                string? email = queryDict.ContainsKey("email") ? Convert.ToString(queryDict["email"][0]) : null;
                if (email != null)
                {
                    bool emails = (queryDict["email"].Count() > 1 ? true : false);
                    response += (emails) ? "Only provide One 'email'\n" : ((email != "admin@example.com") ? "Invalid Input for email\n" : string.Empty);
                }
                else
                {
                    response += "An 'email' is Required\n";
                }

                // Evaluate the password parameter
                string? password = queryDict.ContainsKey("password") ? Convert.ToString(queryDict["password"][0]) : null;
                if (password != null)
                {
                    bool passwords = (queryDict["password"].Count() > 1 ? true : false);
                    response += (passwords) ? "Only provide One 'password'\n" : ((password != "admin1234") ? "Invalid Input for password\n" : string.Empty);
                }
                else
                {
                    response += "An 'password' is Required\n";
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

                // Add response message(s) to the Response
                await httpContext.Response.WriteAsync(response);
            }
            else
            {
                await _next(httpContext);
            }
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


//using Microsoft.AspNetCore.WebUtilities;
//using Microsoft.Extensions.Primitives;

//namespace MiddlewareLoginRedo.Middleware
//{
//    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
//    public class VerifyLoginCreds
//    {
//        private readonly RequestDelegate _next;

//        public VerifyLoginCreds(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext httpContext)
//        {
//            string method = httpContext.Request.Method;
//            string response = string.Empty;

//            if (method == "POST")
//            {
//                //Read response body as stream
//                StreamReader reader = new StreamReader(httpContext.Request.Body);
//                string body = await reader.ReadToEndAsync();

//                //Parse the request body from string into Dictionary
//                Dictionary<string, StringValues> queryDict = QueryHelpers.ParseQuery(body);

//                // extract values form Body into
//                string? email = queryDict.ContainsKey("email") ? Convert.ToString(queryDict["email"][0]) : null;
//                string? password = queryDict.ContainsKey("password") ? Convert.ToString(queryDict["password"][0]) : null;

//                // Make sure that only one 'email' and only one 'password'
//                bool emails = (email != null && queryDict["email"].Count() > 1 ? true : false);
//                response += (emails) ? "Only provide One 'email'\n" : string.Empty;

//                bool passwords = (password != null && queryDict["password"].Count() > 1 ? true : false);
//                response += (passwords) ? "Only Provide One 'password'\n" : string.Empty;

//                // Make sure required parameters are present
//                response += (email == null) ? "'email' is required\n" : string.Empty;
//                response += (password == null) ? "'password' is required\n" : string.Empty;

//                // Make sure required parameter contain valid values
//                if (response == string.Empty)
//                {
//                    response += (email != "admin@example.com") ? "Invalid Input for e-mail\n" : string.Empty;
//                    response += (password != "admin1234") ? "Invalid Input for password\n" : string.Empty;
//                }

//                // If response is still empty at this point, no error conditions were encountered
//                if (response == string.Empty)
//                {
//                    response = "\nSuccessful Login\n";
//                    httpContext.Response.StatusCode = 200;
//                }
//                else
//                {
//                    response += "\nLogin Failed\n";
//                    httpContext.Response.StatusCode = 400;
//                }

//                // Add response message(s) to the Response
//                await httpContext.Response.WriteAsync(response);
//            }
//            else
//            {
//                await _next(httpContext);
//            }
//        }
//    }

//    // Extension method used to add the middleware to the HTTP request pipeline.
//    public static class VerifyLoginCredsExtensions
//    {
//        public static IApplicationBuilder UseVerifyLoginCreds(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<VerifyLoginCreds>();
//        }
//    }
//}
