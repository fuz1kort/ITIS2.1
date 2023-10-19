using MyHttpServer.Attributes;

namespace MyHttpServer.Controllers;

public class GetAttribute : HttpMethodAttribute
{
    public GetAttribute(string actionName) : base(actionName)
    {
    }
}