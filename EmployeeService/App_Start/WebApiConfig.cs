using System.Web.Http;

namespace EmployeeService
{
    using System.Web.Http.Cors;

    /*    public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/Json");
        }
    }*/

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services 

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // --enable cors with parameters origin, header, methods. "*" can be use for ALL
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            config.Filters.Add(new RequireHttpsAttribute());

            // --specify the jsonp formatter 
            // var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            // config.Formatters.Insert(0, jsonpFormatter);

            // --using custom formatter define by the custom class
            // config.Formatters.Add(new CustomJsonFormatter());

            // --This tells ASP.NET Web API to use JsonFormatter when a request is made for text/html which is the default for most browsers.
            // config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            // config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // --removes xml formatter so the api only supports json
            // config.Formatters.Remove(config.Formatters.XmlFormatter);

            // --removes json formatter so the api only supports xml
            // config.Formatters.Remove(config.Formatters.JsonFormatter);
        }
    }
}
