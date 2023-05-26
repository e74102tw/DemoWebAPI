using System.Web.Http;
using WebActivatorEx;
using DemoWebAPI;
using Swashbuckle.Application;
using System.Reflection;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace DemoWebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "DemoWebAPI");
                        c.IncludeXmlComments(GetXmlCommentsPath());
                        c.DescribeAllEnumsAsStrings();
                    })
                .EnableSwaggerUi(c=>
                    {
                        c.InjectStylesheet(thisAssembly, "DemoWebAPI.SwaggerUI.style.css");
                    }
                );
        }
        internal static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\App_Data\XmlDocument.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
