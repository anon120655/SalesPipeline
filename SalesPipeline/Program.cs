using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.FileProviders;
using SalesPipeline.Helpers;
using SalesPipeline.Hubs;
using SalesPipeline.Utils;
using SalesPipeline.ViewModels;
using SalesPipeline.ViewModels.Wrapper;


//***** Not use System.Drawing.Common on .Net7  server  non-Windows support System.Drawing.EnableUnixSupport  *****
//***** FontAwesome <i></i> not use near code c# error Ex. <span><i></i></span>
//***** timestamp Update not use table have count
//***** RestSharp ใช้กับ AuthenticationStateProvider ไม่ได้
// blazorbootstrap 1.9.4 ไม่มีปัญหาเรื่อง Tooltip ค้าง ถ้า 1.9.5-1.10.1 ยังเจอปัญหานี้อยู่ *1.10.2 แก้แล้ว
// Resource temporarily unavailable ต้องไปแก้ DNS servers ใน Webmin 
// if Cascading มีผลทำให้ SEO Meta ไม่แสดง ถ้าไม่ if ตอน reload หน้านั้นๆ ข้อมูลใน Cascading หน้าที่เรียกจะเป็น null ทำให้โปรแกรมค้าง ถ้าไม่ if ต้องไปเช็คเพิ่มใน OnParametersSet
// [JsonIgnore] ใช้ System.Text.Json.Serialization

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();

IConfigurationBuilder con_builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
IConfigurationRoot con_root = con_builder.Build();

var appSettings = con_root.GetSection("AppSettings");
string baseUriApi = con_root["AppSettings:baseUriApi"] ?? String.Empty;
string ServerSite = con_root["AppSettings:ServerSite"] ?? String.Empty;
string ContentRootPath = con_root["AppSettings:ContentRootPath"] ?? String.Empty;

builder.Services.Configure<AppSettings>(appSettings);

//builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IHttpClientCustom, HttpClientCustom>();

//Register ViewModel
builder.Services.AddScoped<ViewModelWrapper>();
builder.Services.AddScoped<UtilsViewModel>();
builder.Services.AddScoped<FileViewModel>();
builder.Services.AddScoped<MasterViewModel>();
builder.Services.AddScoped<ProcessSaleViewModel>();
builder.Services.AddScoped<UserViewModel>();
builder.Services.AddScoped<CustomerViewModel>();
builder.Services.AddScoped<SalesViewModel>();
builder.Services.AddScoped<AssignmentRMViewModel>();
builder.Services.AddScoped<SystemViewModel>();

//StateProvider
builder.Services.AddScoped<AuthorizeViewModel>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthorizeViewModel>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

string currentDirectory = Directory.GetCurrentDirectory();

string currentDirectoryRoot = Path.Combine(currentDirectory, @"wwwroot");
string currentDirectoryFiles = ContentRootPath;

if (ServerSite != ServerSites.DEV && !baseUriApi.Contains("localhost"))
{
	//Server Linix
	if (ServerSite == ServerSites.UAT)
	{
		//currentDirectoryRoot = "/home/thanapat/uat/frontend/wwwroot";
		//currentDirectoryFiles = $"/home/thanapat/uat/files";
	}
	else if (ServerSite == ServerSites.PRO)
	{
		//currentDirectoryRoot = "/home/thanapat/prd/frontend/wwwroot";
		//currentDirectoryFiles = $"/home/thanapat/prd/files";
	}
}

app.UseStaticFiles(new StaticFileOptions() { FileProvider = new PhysicalFileProvider(currentDirectoryRoot) });
app.UseStaticFiles(new StaticFileOptions()
{
	FileProvider = new PhysicalFileProvider(currentDirectoryFiles),
	OnPrepareResponse = ctx =>
	{
		ctx.Context.Response.Headers.Append("Cache-Control","public, max-age=604800");
	}
});

app.UseRouting();

app.MapBlazorHub();
app.MapHub<UserCount>(SignalRUtls.HubUserUrl);
app.MapFallbackToPage("/_Host");

app.Run();
