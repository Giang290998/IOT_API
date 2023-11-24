using IOT_API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IOT_API.Attributes;

public class JwtAuthorizeAttribute : TypeFilterAttribute
{

    public JwtAuthorizeAttribute() : base(typeof(JwtAuthorizeFilter)) { }

}