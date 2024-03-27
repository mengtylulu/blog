using mengtylulu;
using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;
using mengtylulu.ApiModel.DotNet.DependencyInjection.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//常规注入
//builder.Services.AddScoped<ITestScoped, TestScoped>();
//builder.Services.AddSingleton<ITestSingleton, TestSingleton>();
//builder.Services.AddTransient<ITestTransient, TestTransient>();

//实例注入模式-不推荐
builder.Services.AddSingleton<ITestScoped>(new TestScoped());
builder.Services.AddSingleton(new TestScoped() { name = "mty" });

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
