using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using SalesPipeline.Utils.Resources.Authorizes.Auths;

namespace SalesPipeline.Infrastructure.Helpers
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthorizesAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			// skip authorization if action is decorated with [AllowAnonymous] attribute
			var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
			if (allowAnonymous)
				return;

			// authorization
			var user = (UserAuth?)context.HttpContext.Items["User"];
			if (user == null)
			{
				// not logged in or role not authorized
				context.Result = new JsonResult(new {
					Status = StatusCodes.Status401Unauthorized,
					Message = "Unauthorized" 
				}) 
				{ 
					StatusCode = StatusCodes.Status401Unauthorized 
				};
			}
		}
	}
}
