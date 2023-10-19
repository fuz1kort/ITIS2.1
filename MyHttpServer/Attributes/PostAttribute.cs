using MyHttpServer.Attributes;

namespace MyHttpServer.Controllers;

public class PostAttribute : HttpMethodAttribute
{
    public PostAttribute(string actionName) : base(actionName)
    {
    }
}