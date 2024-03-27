using mengtylulu;
using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;
using mengtylulu.ApiModel.DotNet.DependencyInjection.Model;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//����ע��
//builder.Services.AddScoped<ITestScoped, TestScoped>();
//builder.Services.AddSingleton<ITestSingleton, TestSingleton>();
//builder.Services.AddTransient<ITestTransient, TestTransient>();

//ʵ��ע��ģʽ-���Ƽ�
//builder.Services.AddSingleton<ITestScoped>(new TestScoped());
//builder.Services.AddSingleton(new TestScoped() { name = "mty" });

//����ģʽ
//builder.Services.AddScoped(provider => { return new TestScoped(); });
//builder.Services.AddSingleton(provider => { return new TestSingleton(); });
//builder.Services.AddTransient(provider => { return new TestTransient(); });

//����ע��ģʽ
//���IUserService�Ѿ�ע�ᣬͨ��Try***�Ͳ���ע��ɹ���:����ʵ�֣�ֻҪ��ͬһ�����;Ͳ���ע��
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.TryAddScoped<IUserService, UserServiceEx>();
builder.Services.TryAddScoped<IUserService, UserServiceEx>();
//ֻҪ�ǲ�ͬʵ�־���ע��ɹ���ͬһ���ӿڵ���ͬʵ�־Ͳ���ע��ɹ�
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IStudentService, StudentServiceEx>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IStudentService, StudentServiceEx>());

//ɾ������ ��������ע��
//builder.Services.RemoveAll<IStudentService>();

//�滻����, 
builder.Services.Replace(ServiceDescriptor.Singleton<IStudentService,StudentServiceEx>());

//����ģ��ע��
builder.Services.AddScoped(typeof(ITestGeneric<>),typeof(TestGeneric<>));

//����
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
