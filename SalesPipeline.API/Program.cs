using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Repositorys;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Infrastructure.Data.Context;
using SalesPipeline.Utils;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.AspNetCore.Http.Features;
using SalesPipeline.Infrastructure.Data.Mapping;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Utils.ValidationModel;

var builder = WebApplication.CreateBuilder(args);

//[JsonIgnore] ใช้ System.Text.Json.Serialization

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR(e =>
{
	e.MaximumReceiveMessageSize = int.MaxValue; //2147483648 Byte = 2GB
});
builder.Services.Configure<IISServerOptions>(options =>
{
	options.MaxRequestBodySize = int.MaxValue; //2147483648 Byte = 2GB
});

IConfigurationBuilder con_builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
IConfigurationRoot con_root = con_builder.Build();

var appSettings = con_root.GetSection("AppSettings");
var SalesPipelineContext = con_root["ConnectionStrings:SalesPipelineContext"];

//builder.Services.AddDbContext<SalesPipelineContext>(options =>
//options.UseSqlServer(SalesPipelineContext));

//builder.Services.AddDbContext<SalesPipelineContext>(options =>
//	options.UseMySql(SalesPipelineContext, ServerVersion.Parse("10.11.6-MariaDB", ServerType.MariaDb), options => options.UseNetTopologySuite()));

var autoDetectVersion = ServerVersion.AutoDetect(SalesPipelineContext);
//builder.Services.AddDbContext<SalesPipelineContext>(options =>
//	options.UseMySql(SalesPipelineContext,
//				autoDetectVersion,
//				options => options.EnableRetryOnFailure(
//					maxRetryCount: 10,
//					maxRetryDelay: System.TimeSpan.FromSeconds(60),
//					errorNumbersToAdd: null
//					)
//				));

builder.Services.AddDbContext<SalesPipelineContext>(
		   dbContextOptions => dbContextOptions
			   .UseMySql(SalesPipelineContext, autoDetectVersion)
			   .LogTo(Console.WriteLine, LogLevel.Information)
			   .EnableSensitiveDataLogging()
			   .EnableDetailedErrors()
	   );



builder.Services.Configure<AppSettings>(appSettings);
builder.Services.Configure<ApiBehaviorOptions>(options
	=> options.SuppressModelStateInvalidFilter = true);

// configure DI for application services
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddTransient<SalesPipelineContext>();
builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IJwtUtils, JwtUtils>();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<ValidationFilterAttribute>();

//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
	var securitySchema = new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "bearer",
		Reference = new OpenApiReference
		{
			Type = ReferenceType.SecurityScheme,
			Id = "Bearer"
		}
	};
	c.AddSecurityDefinition("Bearer", securitySchema);

	var securityRequirement = new OpenApiSecurityRequirement();
	securityRequirement.Add(securitySchema, new[] { "Bearer" });
	c.AddSecurityRequirement(securityRequirement);

	//add descrtion controller action
	//.csproj file:
	//<PropertyGroup>
	//	<GenerateDocumentationFile>true</GenerateDocumentationFile>
	//	<NoWarn>$(NoWarn);1591</NoWarn>
	//</PropertyGroup>
	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

//Nuget Asp.Versioning.Mvc.ApiExplorer
builder.Services.AddApiVersioning(options =>
{
	options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1.0);
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.ReportApiVersions = true;
}).AddMvc()
  .AddApiExplorer(options =>
  {
	  options.GroupNameFormat = "'v'VVV";
	  options.SubstituteApiVersionInUrl = true;
  });
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
	//Ignore infinity loop class
	options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	//Json return normal First Upper 
	//options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

//RequestSizeLimit FromForm ,IFormFile ,FileByte[] Max
app.Use(async (context, next) =>
{
	var httpMaxRequestBodySizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
	if (httpMaxRequestBodySizeFeature is not null)
		httpMaxRequestBodySizeFeature.MaxRequestBodySize = null; //unlimited I guess
	await next(context);
});

app.UseStaticFiles(new StaticFileOptions()
{
	FileProvider = new PhysicalFileProvider("C:\\DataRM"),
	//OnPrepareResponse = ctx =>
	//{
	//	ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=604800");
	//}
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//	app.UseSwagger();
//	app.UseSwaggerUI();
//}

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

// global cors policy
app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<RequestResponseMiddleware>();
app.MapControllers();

app.Run();
