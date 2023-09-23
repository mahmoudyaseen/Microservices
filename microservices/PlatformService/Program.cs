using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsProduction())
{
    Console.WriteLine("Using Sql Server database");
    var connectionString = builder.Configuration.GetConnectionString("PlatformsConnection");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
}
else
{
    Console.WriteLine("Using In memory database");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("PlatformDb"));
}

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opts => opts.SuppressModelStateInvalidFilter = true);


// builder.Services.AddFluentValidationAutoValidation(opt =>
// {
//     opt.DisableDataAnnotationsValidation = true;
// });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Console.WriteLine($"Command service url is: {app.Configuration["CommandService"]}");

if (app.Environment.IsProduction())
{
    Console.WriteLine("Apply migrations");
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
