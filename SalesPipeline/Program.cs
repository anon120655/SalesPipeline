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
string serverSite = con_root["AppSettings:ServerSite"] ?? String.Empty;
string contentRootPath = con_root["AppSettings:ContentRootPath"] ?? String.Empty;

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
builder.Services.AddScoped<AssignmentBranchViewModel>();
builder.Services.AddScoped<AssignmentCenterViewModel>();
builder.Services.AddScoped<AssignmentRMViewModel>();
builder.Services.AddScoped<ReturnViewModel>();
builder.Services.AddScoped<SystemViewModel>();
builder.Services.AddScoped<DashboardViewModel>();
builder.Services.AddScoped<ExportViewModel>();
builder.Services.AddScoped<MailViewModel>();
builder.Services.AddScoped<LoanViewModel>();
builder.Services.AddScoped<PreCalViewModel>();
builder.Services.AddScoped<PreCalInfoViewModel>();
builder.Services.AddScoped<PreCalStanViewModel>();
builder.Services.AddScoped<PreCalAppViewModel>();
builder.Services.AddScoped<PreCalBusViewModel>();
builder.Services.AddScoped<PreCalWeightViewModel>();
builder.Services.AddScoped<PreCreditViewModel>();
builder.Services.AddScoped<PreChanceViewModel>();
builder.Services.AddScoped<PreFactorViewModel>();
builder.Services.AddScoped<NotifyViewModel>();

//StateProvider
builder.Services.AddScoped<AuthorizeViewModel>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthorizeViewModel>());

builder.Services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

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
string currentDirectoryFiles = contentRootPath;

if (serverSite != ServerSites.DEV && !baseUriApi.Contains("localhost"))
{
	//Server Linix
	if (serverSite == ServerSites.UAT)
	{
		//currentDirectoryRoot = "/home/thanapat/uat/frontend/wwwroot";
		//currentDirectoryFiles = $"/home/thanapat/uat/files";
	}
	else if (serverSite == ServerSites.PRO)
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
