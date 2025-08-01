using mengtylulu;
using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;
using mengtylulu.ApiModel.DotNet.DependencyInjection.Model;
using mengtylulu.Data;
using mengtylulu.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using System.Data.SqlTypes;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//设置全局的时间格式为yyyy-MM-dd HH:mm:ss(ISO 标准格式)
void ConfigureGlobalDateTimeFormat()
{
    CultureInfo cultureInfo = (CultureInfo)CultureInfo.InvariantCulture.Clone();
    cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
    cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
    cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm:ss";

    //设置全局默认文化
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

    Thread.CurrentThread.CurrentCulture = cultureInfo;
    Thread.CurrentThread.CurrentUICulture = cultureInfo;
}
ConfigureGlobalDateTimeFormat();

// Connection DB
//builder.Services.Configure<ConnectionSetting>(builder.Configuration.GetSection("PostgreSqlConnection"));
builder.Services.AddSingleton<NpgsqlDataSource>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
    return NpgsqlDataSource.Create(connectionString ?? string.Empty);
});

//注册通用仓储
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//常规注入
builder.Services.AddScoped<ITestScoped, TestScoped>();
builder.Services.AddScoped<IAnimal, Cat>();
builder.Services.AddSingleton<ITestSingleton, TestSingleton>();
builder.Services.AddTransient<ITestTransient, TestTransient>();

//实例注入模式-不推荐
//builder.Services.AddSingleton<ITestScoped>(new TestScoped());
//builder.Services.AddSingleton(new TestScoped() { name = "mty" });

//工厂模式
//builder.Services.AddScoped(provider => { return new TestScoped(); });
//builder.Services.AddSingleton(provider => { return new TestSingleton(); });
//builder.Services.AddTransient(provider => { return new TestTransient(); });

//排他注入模式
//如果IUserService已经注册，通过Try***就不会注册成功了:不管实现，只要是同一个类型就不能注册
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.TryAddScoped<IUserService, UserServiceEx>();
builder.Services.TryAddScoped<IUserService, UserServiceEx>();
//只要是不同实现就能注册成功，同一个接口的相同实现就不能注册成功
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IStudentService, StudentServiceEx>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IStudentService, StudentServiceEx>());

//删除服务 后续不能注册
//builder.Services.RemoveAll<IStudentService>();

//替换服务, 
builder.Services.Replace(ServiceDescriptor.Singleton<IStudentService, StudentServiceEx>());

//泛型模板注册
builder.Services.AddScoped(typeof(ITestGeneric<>), typeof(TestGeneric<>));

//跨域
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    builder.SetIsOriginAllowed(_ => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", (HttpRequest request) =>
{
    var userAgent = request.Headers.UserAgent;
    var customHeader = request.Headers["x-custom-header"];
    return Results.Ok(new { userAgent = userAgent, customHeader = customHeader });
});

app.Run();
