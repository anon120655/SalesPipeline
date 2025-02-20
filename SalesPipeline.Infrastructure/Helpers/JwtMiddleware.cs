using Microsoft.AspNetCore.Http;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Helpers
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;

		public JwtMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context, IRepositoryWrapper repo)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			var userId = repo.jwtUtils.ValidateJwtToken(token);
			if (userId != null)
			{
				// attach user to context on successful jwt validation
				context.Items["User"] = repo.Authorizes.GetById(userId.Value);
			}

			await _next(context);
		}
	}
}
