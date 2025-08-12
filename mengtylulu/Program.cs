using mengtylulu;
using mengtylulu.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using System.Data.SqlTypes;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//����ȫ�ֵ�ʱ���ʽΪyyyy-MM-dd HH:mm:ss(ISO ��׼��ʽ)
void ConfigureGlobalDateTimeFormat()
{
    CultureInfo cultureInfo = (CultureInfo)CultureInfo.InvariantCulture.Clone();
    cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
    cultureInfo.DateTimeFormat.LongTimePattern = "HH:mm:ss";
    cultureInfo.DateTimeFormat.ShortTimePattern = "HH:mm:ss";

    //����ȫ��Ĭ���Ļ�
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

//ע��ͨ�òִ�
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//ʵ��ע��ģʽ-���Ƽ�
//builder.Services.AddSingleton<ITestScoped>(new TestScoped());
//builder.Services.AddSingleton(new TestScoped() { name = "mty" });

//����ģʽ
//builder.Services.AddScoped(provider => { return new TestScoped(); });
//builder.Services.AddSingleton(provider => { return new TestSingleton(); });
//builder.Services.AddTransient(provider => { return new TestTransient(); });



//ɾ������ ��������ע��
//builder.Services.RemoveAll<IStudentService>();

//�滻����, 
//builder.Services.Replace(ServiceDescriptor.Singleton<IStudentService, StudentServiceEx>());

//����ģ��ע��
//builder.Services.AddScoped(typeof(ITestGeneric<>), typeof(TestGeneric<>));

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
