using DataService.Data;
using DataService.Extensions;
using DataService.Helpers;
using DataService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDataRepo, DataRepo>();
builder.Services.AddScoped<IIdentityDataClient, IdentityDataClient>();
//builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DataConn")));
}
else
{
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMem"));

    //Console.WriteLine("--> Using SqlServer Db");
    //builder.Services.AddDbContext<AppDbContext>(opt =>
    //    opt.UseSqlServer(builder.Configuration.GetConnectionString("DataConn")));
}
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseRouting();

ExceptionMiddlewareExtensions.ConfigureCustomExceptionMiddleware(app);

app.UseAuthorization();
// configure HTTP request pipeline
{
    // global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());


    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();
    // global error handler
    //app.UseMiddleware<ErrorHandlerMiddleware>();
    app.MapControllers();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGrpcService<GrpcDataService>();

    endpoints.MapGet("/protos/data.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Protos/data.proto"));
    });
});

PublishDb.PublishPopulation(app, builder.Configuration, app.Environment.IsProduction());

app.Run();
