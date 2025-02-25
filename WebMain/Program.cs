using Serilog;
using WebMain;

var builder = WebApplication.CreateBuilder(args);

// ��������� ������������ �� ���������� ���������
builder.Configuration.AddEnvironmentVariables();

// ��������� Serilog
Log.Logger = new LoggerConfiguration().Configure(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// ��������� middleware pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

// ��������������� � ��������� ������ �� Swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html");
        return;
    }
    await next();
});

app.Run();
