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
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Utils.ValidationModel;
using Hangfire;
using System.Text.Json;
using Hangfire.MySql;
using SalesPipeline.Infrastructure.Data.Logger.Context;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

//[JsonIgnore] ใช้ System.Text.Json.Serialization

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
var SalesPipelineLogContext = con_root["ConnectionStrings:SalesPipelineLogContext"];
var contentRootPath = con_root["AppSettings:ContentRootPath"] ?? String.Empty;

var autoDetectVersion = ServerVersion.AutoDetect(SalesPipelineContext);

builder.Services.AddDbContext<SalesPipelineContext>(
           dbContextOptions => dbContextOptions
               .UseMySql(SalesPipelineContext, autoDetectVersion)
               .LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors()
       );

builder.Services.AddDbContext<SalesPipelineLogContext>(
           dbContextOptions => dbContextOptions
               .UseMySql(SalesPipelineLogContext, autoDetectVersion)
               .LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors()
       );


var SalesPipelineJobContext = con_root["ConnectionStrings:SalesPipelineJobContext"];

//**แก้วิธีให้แสดงแดชบอร์ดทั้งหมดไม่ได้ บางหน้าเป็นหน้าว่าง กำหนด Allow User Variables=true ใน ConnectionStrings
builder.Services.AddHangfire(config =>
    config.UseStorage(new MySqlStorage(SalesPipelineJobContext, new MySqlStorageOptions
    {
        TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted, // ตั้งค่าระดับการแยกการทำธุรกรรม
        QueuePollInterval = TimeSpan.FromSeconds(15), // ตั้งค่าช่วงเวลาสำหรับการเช็คคิว
        JobExpirationCheckInterval = TimeSpan.FromHours(1), // ตั้งค่าช่วงเวลาสำหรับการตรวจสอบงานที่หมดอายุ
        CountersAggregateInterval = TimeSpan.FromMinutes(5), // ตั้งค่าช่วงเวลาสำหรับการรวมผลของเคาน์เตอร์
        PrepareSchemaIfNecessary = true, // ให้สร้าง schema ถ้าจำเป็น
        DashboardJobListLimit = 5000, // จำนวนสูงสุดของรายการงานที่จะแสดงใน dashboard
        TransactionTimeout = TimeSpan.FromMinutes(1), // ตั้งค่าเวลา timeout สำหรับธุรกรรม
        TablesPrefix = "Hangfire_"
    }))
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseColouredConsoleLogProvider()
);
builder.Services.AddHangfireServer();

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

builder.Services.AddSingleton<DatabaseBackupService>();

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3 });

builder.Services.Configure<AppSettings>(appSettings);
builder.Services.Configure<ApiBehaviorOptions>(options
    => options.SuppressModelStateInvalidFilter = true);

// configure DI for application services
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddTransient<SalesPipelineContext>();
builder.Services.AddTransient<SalesPipelineLogContext>();
builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddSingleton<NotificationService>();

builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

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

    #pragma warning disable S3887
        var bearer = new[] { "Bearer" };
    #pragma warning restore S3887

    var securityRequirement = new OpenApiSecurityRequirement();
    securityRequirement.Add(securitySchema, bearer);
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

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificMethods",
        builder =>
        {
            builder.WithMethods("GET", "POST", "PUT", "DELETE")
                   .AllowAnyHeader();
        });
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
    FileProvider = new PhysicalFileProvider(contentRootPath),
});

app.UseSwagger();
app.UseSwaggerUI();

// ปิดใช้ชั่วคราวเพื่อทดสอบบน prod ต้องเปิด
//app.UseHttpsRedirection();

app.UseAuthorization();

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseCors("AllowSpecificMethods");

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<RequestResponseMiddleware>();
app.MapControllers();

// กำหนดให้ใช้ Hangfire middleware พร้อมการตั้งค่าการรับรองความถูกต้อง
app.UseHangfireDashboard("/hangfire/dashboard", new DashboardOptions
{
    Authorization = new[] { new MyAuthorizationFilter() }
});

// Schedule the job to run every hour
RecurringJob.AddOrUpdate<DatabaseBackupService>("backup-job", x => x.BackupDatabase(), Cron.Hourly);

await app.RunAsync();
